using System;
using System.IO;

namespace SimpleATM
{
    class Program
    {
        static void ClearScreen()
        {
            Console.Clear();
        }

        static void WaitForEnter()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }


        static bool IsGreaterOrEqual<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) >= 0;
        }

        class BankAccount
        {
            public string AccountNumber { get; set; }
            public int Pin { get; set; }
            public double Balance { get; set; }
            public string FilePath { get; set; }

            public BankAccount(string filePath)
            {
                FilePath = filePath;
                if (!File.Exists(FilePath))
                {
                    Console.WriteLine("Cannot open file: " + FilePath);
                    Environment.Exit(1);
                }

                string[] lines = File.ReadAllLines(FilePath);
                AccountNumber = lines[0];
                Pin = int.Parse(lines[1]);
                Balance = double.Parse(lines[2]);
            }

            public void Save()
            {
                string[] lines = {
                    AccountNumber,
                    Pin.ToString(),
                    Balance.ToString("F2")
                };
                File.WriteAllLines(FilePath, lines);
            }

            public virtual bool Deposit(double amount)
            {
                if (amount > 0)
                {
                    Balance += amount;
                    Save();
                    return true;
                }
                return false;
            }

            public virtual bool Withdraw(double amount)
            {
                const double fee = 1.0;
                if (amount > 0 && IsGreaterOrEqual(Balance, amount + fee))
                {
                    Balance -= (amount + fee);
                    Save();
                    return true;
                }
                return false;
            }

            public virtual string GetAccountType()
            {
                return "Generic Account";
            }

            public bool CheckPin(int inputPin)
            {
                return inputPin == Pin;
            }
        }

        class SavingsAccount : BankAccount
        {
            public SavingsAccount(string filePath) : base(filePath) { }

            public override bool Withdraw(double amount)
            {
                const double fee = 1.0;
                const double minBalance = 100.0;
                if (amount > 0 && IsGreaterOrEqual(Balance - amount - fee, minBalance))
                {
                    Balance -= (amount + fee);
                    Save();
                    return true;
                }
                return false;
            }

            public override string GetAccountType()
            {
                return "Savings Account";
            }
        }

        class CheckingAccount : BankAccount
        {
            public CheckingAccount(string filePath) : base(filePath) { }

            public override bool Deposit(double amount)
            {
                const double fee = 1.0;
                if (amount > fee)
                {
                    Balance += (amount - fee);
                    Save();
                    return true;
                }
                return false;
            }

            public override string GetAccountType()
            {
                return "Checking Account";
            }
        }

        static void Main(string[] args)
        {
            ClearScreen();
            Console.WriteLine("Welcome to the ATM");
            Console.Write("Choose account type (savings/checking): ");
            string type = Console.ReadLine().ToLower();

            BankAccount account = null;

            if (type == "savings")
            {
                account = new SavingsAccount("files/savings.txt");
            }
            else if (type == "checking")
            {
                account = new CheckingAccount("files/checking.txt");
            }
            else
            {
                Console.WriteLine("Invalid account type. Bye!");
                return;
            }

            Console.Write("Enter your PIN: ");
            int inputPin = int.Parse(Console.ReadLine());

            if (!account.CheckPin(inputPin))
            {
                Console.WriteLine("Wrong PIN. Bye!");
                return;
            }

            int choice = 0;
            while (choice != 5)
            {
                ClearScreen();
                Console.WriteLine("1. Check balance");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Show account type");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        ClearScreen();
                        Console.WriteLine($"Balance: ${account.Balance:F2}");
                        WaitForEnter();
                        break;

                    case 2:
                        ClearScreen();
                        Console.Write("Enter amount to deposit: $");
                        double deposit = double.Parse(Console.ReadLine());
                        if (account.Deposit(deposit))
                            Console.WriteLine("Deposit successful!");
                        else
                            Console.WriteLine("Deposit failed.");
                        WaitForEnter();
                        break;

                    case 3:
                        ClearScreen();
                        Console.Write("Enter amount to withdraw: $");
                        double withdraw = double.Parse(Console.ReadLine());
                        if (account.Withdraw(withdraw))
                            Console.WriteLine("Withdrawal successful (with $1 fee)!");
                        else
                            Console.WriteLine("Withdrawal failed.");
                        WaitForEnter();
                        break;

                    case 4:
                        ClearScreen();
                        Console.WriteLine("Account type: " + account.GetAccountType());
                        WaitForEnter();
                        break;

                    case 5:
                        ClearScreen();
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        WaitForEnter();
                        break;
                }
            }
        }
    }
}

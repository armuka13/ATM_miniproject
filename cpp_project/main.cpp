#include <iostream>
#include <fstream>
#include <string>
#include <iomanip>
#include <cstdlib>

using namespace std;


template <typename T>
bool isGreaterOrEqual(T a, T b) {
    return a >= b;
}

void clearScreen() {
    system("cls"); 
}

void waitForEnter() {
    cout << "\nPress Enter to continue...";
    cin.ignore();
    cin.get();
}

class BankAccount {
public:
    string accountNumber;
    int pin;
    double balance;
    string filename;

    BankAccount(string file) {
        filename = file;
        ifstream fin(filename);
        if (!fin) {
            cout << "Cannot open file " << filename << endl;
            exit(1);
        }
        getline(fin, accountNumber);
        fin >> pin >> balance;
        fin.close();
    }

    void save() {
        ofstream fout(filename);
        fout << accountNumber << "\n" << pin << "\n" << fixed << setprecision(2) << balance << "\n";
        fout.close();
    }

    bool checkPin(int inputPin) {
        return inputPin == pin;
    }

    virtual bool deposit(double amount) {
        if (amount > 0) {
            balance += amount;
            save();
            return true;
        }
        return false;
    }

    virtual bool withdraw(double amount) {
        const double fee = 1.0;
        if (amount > 0 && isGreaterOrEqual(balance, amount + fee)) {
            balance -= (amount + fee);
            save();
            return true;
        }
        return false;
    }

    virtual string getType() {
        return "Unknown Account";
    }
};

class SavingsAccount : public BankAccount {
public:
    SavingsAccount(string file) : BankAccount(file) {}

    bool withdraw(double amount) override {
        const double fee = 1.0;
        const double minBalance = 100.0;
        if (amount > 0 && isGreaterOrEqual(balance - amount - fee, minBalance)) {
            balance -= (amount + fee);
            save();
            return true;
        }
        return false;
    }

    string getType() override {
        return "Savings Account";
    }
};

class CheckingAccount : public BankAccount {
public:
    CheckingAccount(string file) : BankAccount(file) {}

    bool deposit(double amount) override {
        const double depositFee = 1.0;
        if (amount > depositFee) {
            balance += (amount - depositFee);
            save();
            return true;
        }
        return false;
    }

    string getType() override {
        return "Checking Account";
    }
};

int main() {
    clearScreen();
    cout << "Welcome to the ATM\n";
    cout << "Choose account type (savings/checking): ";
    string type;
    getline(cin, type);

    BankAccount* account = nullptr;

    if (type == "savings") {
        account = new SavingsAccount("files/savings.txt");
    } else if (type == "checking") {
        account = new CheckingAccount("files/checking.txt");
    } else {
        cout << "Invalid account type. Bye!\n";
        return 1;
    }

    cout << "Enter your PIN: ";
    int pinInput;
    cin >> pinInput;
    cin.ignore();

    if (!account->checkPin(pinInput)) {
        cout << "Wrong PIN. Bye!\n";
        return 1;
    }

    int choice = 0;
    while (choice != 5) {
        clearScreen();
        cout << "1. Check balance\n";
        cout << "2. Deposit\n";
        cout << "3. Withdraw\n";
        cout << "4. Show account type\n";
        cout << "5. Exit\n";
        cout << "Choose an option: ";
        cin >> choice;
        cin.ignore();

        if (choice == 1) {
            clearScreen();
            cout << "Balance: $" << fixed << setprecision(2) << account->balance << "\n";
            waitForEnter();
        } else if (choice == 2) {
            clearScreen();
            cout << "Enter amount to deposit: $";
            double amount;
            cin >> amount;
            cin.ignore();
            if (account->deposit(amount)) {
                cout << "Deposit successful!\n";
            } else {
                cout << "Deposit failed.\n";
            }
            waitForEnter();
        } else if (choice == 3) {
            clearScreen();
            cout << "Enter amount to withdraw: $";
            double amount;
            cin >> amount;
            cin.ignore();
            if (account->withdraw(amount)) {
                cout << "Withdrawal successful!\n";
            } else {
                cout << "Withdrawal failed (check balance or minimum requirements).\n";
            }
            waitForEnter();
        } else if (choice == 4) {
            clearScreen();
            cout << "Account type: " << account->getType() << "\n";
            waitForEnter();
        } else if (choice == 5) {
            clearScreen();
            cout << "Goodbye!\n";
        } else {
            cout << "Invalid choice. Try again.\n";
            waitForEnter();
        }
    }


    return 0;
}

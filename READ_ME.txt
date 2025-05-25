Simple ATM Program

Overview
This is a basic console-based ATM program implemented in C++ and C#. It demonstrates core object-oriented programming (OOP) concepts like inheritance and polymorphism using two account types: SavingsAccount and CheckingAccount. The program supports deposit, withdrawal, balance checking, and account type display.

Login Details
- For savings: enter `savings` as account type and PIN `1234`
- For checking: enter `checking` as account type and PIN `4321`

Features
- SavingsAccount
  - Requires maintaining a minimum balance of $100 after withdrawals.
  - Withdrawals that leave the balance below $100 are not allowed.

- CheckingAccount
  - Charges a $1 fee on each deposit.
  - Deposits must be greater than $1 to succeed.
  - Charges a $1 fee on each withdrawal.

- PIN verification for secure account access.
- Account data (account number, PIN, balance) is stored in simple `.txt` files.
- Screen is cleared after each operation for a clean interface.


Options Menu
1. Check balance  
2. Deposit (Checking accounts deduct a $1 fee)  
3. Withdraw (Savings accounts must keep at least $100 after withdrawal)  
4. Display account type  
5. Exit

Notes
If the files in C++ project do not open, you should change the directory of the files using the absolute path instead of the relative one.

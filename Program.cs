using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
/*
EPIC MODE
------------------------------------------------
==>> Add the ability to have more than one user. Take the time to plan out the changes needed as there are many ways to solve this problem. Include changes to the interface as well as changes to how you store the data.
==>> Add simple password protection.
------------------------------------------------
*/

namespace FirstBankOfSuncoast
{

    class User
    {
        public string UserID { get; set; }
        public string PassWord { get; set; }
    }
    //--------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------//
    class UserRepository
    {
        private List<User> users = new List<User>();
        public User user = new User();

        public void LoadUserRepository()
        {
            if (File.Exists("userrepository.csv"))
            {
                Console.WriteLine("Loading user info...");
                Console.WriteLine();
                var fileReader = new StreamReader("userrepository.csv");
                var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                users = csvReader.GetRecords<User>().ToList();
                fileReader.Close();
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//

        public void SaveUserRepository()
        {
            Console.WriteLine();
            Console.WriteLine("Saving user info...");
            var fileWriter = new StreamWriter("userrepository.csv");
            var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(users);
            fileWriter.Close();
        }
        //FINISH THESE FUNCTIONS
        public void RequestUserIDAndPassword()
        {

            return;
        }
        public void SetUserIDAndPassword()
        {
            var keepAsking = true;
            var counter = 0;
            while (keepAsking)
            {
                Console.WriteLine("Please Enter Your New Username Below:");
                Console.Write("(Usernames are case-sensitive)");
                var newUserID = Console.ReadLine();
                if (users.Any(user => user.UserID == newUserID))
                {
                    Console.Clear();
                    Console.WriteLine("That username is taken. Try again.");
                    counter += 1;
                    continue;
                }
                else if (counter == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Sorry, that username is taken. You have to choose a different one.");
                    Console.WriteLine("Do you want to try again?");
                    Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the Log-In Menu) ");
                    var choice2 = Console.ReadLine().ToUpper();
                    if (choice2 == "Y")
                    {
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        Console.Clear();
                        keepAsking = false;
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Available username selected.");
                    Console.WriteLine();
                    keepAsking = false;
                    var keepAsking2 = true;
                    var counter2 = 0;
                    while (keepAsking2)
                    {
                        Console.Clear();
                        Console.WriteLine("Please Enter Your New Password Below:");
                        Console.Write("(Passwords are case-sensitive and must contain a number, a lowercase letter, a capital letter, and be at least 8 characters long)");
                        var newPassWord = Console.ReadLine();
                        if (newPassWord.Count() >= 8 && newPassWord.Any(char.IsLower) && newPassWord.Any(char.IsUpper) && newPassWord.Any(char.IsDigit))
                        {
                            Console.WriteLine();
                            Console.WriteLine("Valid password selected.");
                            Console.WriteLine("Returning to Log-In Menu");
                            user.UserID = newUserID;
                            user.PassWord = newPassWord;
                            users.Add(user);
                            keepAsking2 = false;
                        }
                        else if (counter2 == 3)
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Sorry, that username is taken. You have to choose a different one.");
                            Console.WriteLine("Do you want to try again?");
                            Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the Log-In Menu) ");
                            var choice2 = Console.ReadLine().ToUpper();
                            if (choice2 == "Y")
                            {
                                Console.Clear();
                                continue;
                            }
                            else
                            {
                                Console.Clear();
                                keepAsking2 = false;
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("That password was invalid. Try again.");
                            counter2 += 1;
                            continue;
                        }
                    }
                }
            }
            // public void ResetPassword()
            // {

            // }
            // public void ForgotPassword()
            // {

            // }
            // public void ForgotUserID()
            // {

            // }
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        class Transaction
        {
            public string TransactionType { get; set; }
            public string Account { get; set; }
            public double Amount { get; set; }
            public string UserID { get; set; }// ==>> Put User ID on Transaction, Then Only One File Needed for Account Info and One File Needed for Transaction Info, Change Other Functions So They Take Into Account User ID or change list of transactions set to transactions to user ID from outset of Main via LINQ function
                                              // public int ID { get; set; } >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> could set ID every time transaction is added to database and increment to total transactions easily in database but also could count transfer as one transaction and assign same ID to corresponding withdraw and transfer
            public DateTime TimeOfTransaction { get; set; } = DateTime.Now;
            //--------------------------------------------------------------------------------------------------------------------------------//
            public string Description()
            {
                if (TransactionType == "withdraw")
                {
                    return $"${Amount} was withdrawn from {Account} account at {TimeOfTransaction}.";
                }
                else if (TransactionType == "withdraw for transfer")
                {
                    return $"${Amount} was withdrawn for transfer from {Account} account at {TimeOfTransaction}.";
                }
                else if (TransactionType == "deposit for transfer")
                {
                    return $"${Amount} was deposited for transfer into {Account} account at {TimeOfTransaction}.";
                }
                else
                {
                    return $"${Amount} was deposited into {Account} account at {TimeOfTransaction}.";
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        class UserTransactions //This system would only work for one user. Would need to implement something to allow the database to select from different files to extend it to different users. 
        {
            private List<Transaction> transactions = new List<Transaction>();

            public void LoadTransactionsFromCSV()
            {
                if (File.Exists("transactions.csv"))
                {
                    Console.WriteLine("Loading database...");
                    Console.WriteLine();
                    var fileReader = new StreamReader("transactions.csv");
                    var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                    transactions = csvReader.GetRecords<Transaction>().ToList();
                    fileReader.Close();
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public void SaveTransactionsToCSV()
            {
                Console.WriteLine();
                Console.WriteLine("Saving to database...");
                var fileWriter = new StreamWriter("transactions.csv");
                var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(transactions);
                fileWriter.Close();
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public void DepositToSavings(User user)
            {
                var newTransaction = new Transaction();
                newTransaction.TransactionType = "deposit";
                newTransaction.Account = "savings";
                newTransaction.UserID = user.UserID;
                var keepAsking = true;
                while (keepAsking)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much money would you like to deposit into your savings account? ");
                    double amountMoney;
                    var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                    if (isThisGoodInput && amountMoney > 0)
                    {
                        newTransaction.Amount = amountMoney;
                        transactions.Add(newTransaction);
                        SaveTransactionsToCSV();
                        Console.WriteLine();
                        Console.WriteLine("Funds deposited to savings account.");
                        keepAsking = false;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                        Console.WriteLine();
                    }
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public void DepositToChecking(User user)
            {
                var newTransaction = new Transaction();
                newTransaction.TransactionType = "deposit";
                newTransaction.Account = "checking";
                newTransaction.UserID = user.UserID;
                var keepAsking = true;
                while (keepAsking)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much money would you like to deposit into your checking account? ");
                    double amountMoney;
                    var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                    if (isThisGoodInput && amountMoney > 0)
                    {
                        newTransaction.Amount = amountMoney;
                        transactions.Add(newTransaction);
                        SaveTransactionsToCSV();
                        Console.WriteLine();
                        Console.WriteLine("Funds deposited to checking account.");
                        keepAsking = false;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                        Console.WriteLine();
                    }
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public void WithdrawFromSavings(User user)
            {
                var newTransaction = new Transaction();
                newTransaction.TransactionType = "withdraw";
                newTransaction.Account = "savings";
                newTransaction.UserID = user.UserID;
                var keepAsking = true;
                var counter = 0;
                while (keepAsking)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much money would you like to withdraw from your savings account? ");
                    double amountMoney;
                    var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                    if (isThisGoodInput && amountMoney > 0 && ((savingsTotal(user) - amountMoney) >= 0))
                    {
                        newTransaction.Amount = amountMoney;
                        transactions.Add(newTransaction);
                        SaveTransactionsToCSV();
                        Console.WriteLine();
                        Console.WriteLine("Funds withdrawn from savings account.");
                        keepAsking = false;
                    }
                    else if (counter == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input.");
                        Console.WriteLine();
                        Console.WriteLine("Either: (1) your withdrawal amount will cause your account to go negative,");
                        Console.WriteLine("        (2) you entered a negative number,");
                        Console.WriteLine("     Or (3) you entered a non-number.");
                        Console.WriteLine();
                        Console.WriteLine("Do you want to try again?");
                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                        var choice = Console.ReadLine().ToUpper();
                        if (choice == "Y")
                        {
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            keepAsking = false;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                        Console.WriteLine();
                        counter += 1;
                    }
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public void WithdrawFromChecking(User user)
            {
                var newTransaction = new Transaction();
                newTransaction.TransactionType = "withdraw";
                newTransaction.Account = "checking";
                newTransaction.UserID = user.UserID;
                var keepAsking = true;
                var counter = 0;
                while (keepAsking)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much money would you like to withdraw from your checking account? ");
                    double amountMoney;
                    var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                    if (isThisGoodInput && amountMoney > 0 && ((checkingTotal(user) - amountMoney) >= 0))
                    {
                        newTransaction.Amount = amountMoney;
                        transactions.Add(newTransaction);
                        SaveTransactionsToCSV();
                        Console.WriteLine();
                        Console.WriteLine("Funds withdrawn from checking account.");
                        keepAsking = false;
                    }
                    else if (counter == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input.");
                        Console.WriteLine();
                        Console.WriteLine("Either: (1) your withdrawal amount will cause your account to go negative,");
                        Console.WriteLine("        (2) you entered a negative number,");
                        Console.WriteLine("     Or (3) you entered a non-number.");
                        Console.WriteLine();
                        Console.WriteLine("Do you want to try again?");
                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                        var choice = Console.ReadLine().ToUpper();
                        if (choice == "Y")
                        {
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            keepAsking = false;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                        Console.WriteLine();
                        counter += 1;
                    }
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public void TransferFromChecking(User user)
            {
                var keepAsking = true;
                var counter = 0;
                while (keepAsking)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much money would you like to transfer from checking to savings? ");
                    double amountMoney;
                    var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                    if (isThisGoodInput && amountMoney > 0 && ((checkingTotal(user) - amountMoney) >= 0))
                    {
                        var newTransaction = new Transaction();
                        newTransaction.TransactionType = "withdraw for transfer";
                        newTransaction.Account = "checking";
                        newTransaction.UserID = user.UserID;
                        newTransaction.Amount = amountMoney;
                        transactions.Add(newTransaction);
                        SaveTransactionsToCSV();
                        Console.WriteLine();
                        Console.WriteLine("Funds withdrawn from checking account.");
                        var newTransaction2 = new Transaction();
                        newTransaction2.TransactionType = "deposit for transfer";
                        newTransaction2.Account = "savings";
                        newTransaction2.UserID = user.UserID;
                        Console.WriteLine();
                        newTransaction2.Amount = amountMoney;
                        transactions.Add(newTransaction2);
                        SaveTransactionsToCSV();
                        Console.WriteLine();
                        Console.WriteLine("Funds deposited to savings account.");
                        Console.WriteLine();
                        keepAsking = false;
                    }
                    else if (counter == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input.");
                        Console.WriteLine();
                        Console.WriteLine("Either: (1) your transfer amount will cause your account to go negative,");
                        Console.WriteLine("        (2) you entered a negative number,");
                        Console.WriteLine("     Or (3) you entered a non-number.");
                        Console.WriteLine();
                        Console.WriteLine("Do you want to try again?");
                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                        var choice = Console.ReadLine().ToUpper();
                        if (choice == "Y")
                        {
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            keepAsking = false;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                        Console.WriteLine();
                        counter += 1;
                    }
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public void TransferFromSavings(User user)
            {
                var keepAsking = true;
                var counter = 0;
                while (keepAsking)
                {
                    Console.WriteLine();
                    Console.WriteLine("How much money would you like to transfer from savings to checking? ");
                    double amountMoney;
                    var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                    if (isThisGoodInput && amountMoney > 0 && ((savingsTotal(user) - amountMoney) >= 0))
                    {
                        var newTransaction = new Transaction();
                        newTransaction.TransactionType = "withdraw for transfer";
                        newTransaction.Account = "savings";
                        newTransaction.UserID = user.UserID;
                        newTransaction.Amount = amountMoney;
                        transactions.Add(newTransaction);
                        SaveTransactionsToCSV();
                        Console.WriteLine();
                        Console.WriteLine("Funds withdrawn from savings account.");
                        var newTransaction2 = new Transaction();
                        newTransaction2.TransactionType = "deposit for transfer";
                        newTransaction2.Account = "checking";
                        newTransaction2.UserID = user.UserID;
                        Console.WriteLine();
                        newTransaction2.Amount = amountMoney;
                        transactions.Add(newTransaction2);
                        SaveTransactionsToCSV();
                        Console.WriteLine();
                        Console.WriteLine("Funds deposited to checking account.");
                        Console.WriteLine();
                        keepAsking = false;
                    }
                    else if (counter == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input.");
                        Console.WriteLine();
                        Console.WriteLine("Either: (1) your transfer amount will cause your account to go negative,");
                        Console.WriteLine("        (2) you entered a negative number,");
                        Console.WriteLine("     Or (3) you entered a non-number.");
                        Console.WriteLine();
                        Console.WriteLine("Do you want to try again?");
                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                        var choice = Console.ReadLine().ToUpper();
                        if (choice == "Y")
                        {
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            keepAsking = false;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                        Console.WriteLine();
                        counter += 1;
                    }
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public void GetAllTransactions(User user)
            {
                foreach (var transaction in transactions.Where(transaction => transaction.UserID == user.UserID))
                {
                    Console.WriteLine(transaction.Description());
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public double savingsTotal(User user)
            {
                var depositSum = transactions.Where(transaction => transaction.TransactionType == "deposit" && transaction.UserID == user.UserID && transaction.Account == "savings").Sum(transaction => transaction.Amount);
                var withdrawalSum = transactions.Where(transaction => transaction.TransactionType == "withdraw" && transaction.UserID == user.UserID && transaction.Account == "savings").Sum(transaction => transaction.Amount);
                var savingsBalance = depositSum - withdrawalSum;
                return savingsBalance;
            }
            //--------------------------------------------------------------------------------------------------------------------------------//
            //--------------------------------------------------------------------------------------------------------------------------------//
            public double checkingTotal(User user)
            {
                var depositSum = transactions.Where(transaction => transaction.TransactionType == "deposit" && transaction.UserID == user.UserID && transaction.Account == "checking").Sum(transaction => transaction.Amount);
                var withdrawalSum = transactions.Where(transaction => transaction.TransactionType == "withdraw" && transaction.UserID == user.UserID && transaction.Account == "checking").Sum(transaction => transaction.Amount);
                var checkingBalance = depositSum - withdrawalSum;
                return checkingBalance;
            }
        }
        class Program
        {

            static void Greeting()
            {
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine("Welcome to the First Bank of Suncoast's Account Management Application");
                Console.WriteLine("----------------------------------------------------------------------");
            }
            static string Menu()
            {
                Console.WriteLine();
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("(C)heck Account Balance");
                Console.WriteLine("(D)eposit Funds");
                Console.WriteLine("(W)ithdraw Funds");
                Console.WriteLine("(T)ransfer Funds");
                // Console.WriteLine("(T)ransfer Funds");
                Console.WriteLine("(V)iew All Transactions");
                Console.WriteLine("(Q)uit the Application");
                var choice = Console.ReadLine().ToUpper();
                return choice;
            }

            static void Main(string[] args)
            {
                var users = new UserRepository();
                users.LoadUserRepository();
                var keepGoing = true;
                var counter1 = 0;
                Greeting();
                Console.WriteLine();
                while (keepGoing)
                {
                    Console.Write("(L)og In or (C)reate New Account (Press L or C then press Enter) ");
                    var choice = Console.ReadLine().ToUpper();
                    if (choice == "L")
                    {
                        Console.Clear();
                        users.RequestUserIDAndPassword();
                        break;
                    }
                    else if (choice == "C")
                    {
                        Console.Clear();
                        users.SetUserIDAndPassword();
                        continue;
                    }
                    else if (counter1 == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input. You have to press L or C.");
                        Console.WriteLine("Do you want to try again?");
                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                        var choice2 = Console.ReadLine().ToUpper();
                        if (choice2 == "Y")
                        {
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            keepGoing = false;
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                        Console.WriteLine();
                        counter1 += 1;
                    }
                }
                if (keepGoing)
                {
                    var userDatabase = new UserTransactions();
                    userDatabase.LoadTransactionsFromCSV();
                    var keepGoing2 = true;
                    while (keepGoing2)
                    {
                        var menuSelection = Menu();
                        switch (menuSelection)
                        {
                            case "C":
                                Console.Clear();
                                Console.WriteLine();
                                Console.Write("Savings or Checking? (Press C or S then press Enter): ");
                                var accountSelection1 = Console.ReadLine().ToUpper();
                                Console.WriteLine();
                                var keepAsking1 = true;
                                var counter2 = 0;
                                while (keepAsking1)
                                {
                                    if (accountSelection1 == "S")
                                    {
                                        Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal(user)}");
                                        keepAsking1 = false;
                                    }
                                    else if (accountSelection1 == "C")
                                    {
                                        Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal(user)}");
                                        keepAsking1 = false;
                                    }
                                    else if (counter2 == 3)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Sorry, that isn't a valid input. You have to press S or C.");
                                        Console.WriteLine("Do you want to try again?");
                                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                                        var choice2 = Console.ReadLine().ToUpper();
                                        if (choice2 == "Y")
                                        {
                                            Console.Clear();
                                            continue;
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            keepAsking1 = false;
                                        }
                                        Console.WriteLine();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                                        Console.WriteLine();
                                        counter2 += 1;
                                    }
                                }
                                break;
                            case "D":
                                Console.Clear();
                                Console.WriteLine("");
                                Console.Write("Savings or Checking? (Press C or S then press Enter): ");
                                var accountSelection2 = Console.ReadLine().ToUpper();
                                var counter3 = 0;
                                var keepAsking2 = true;
                                while (keepAsking2)
                                {
                                    if (accountSelection2 == "S")
                                    {
                                        userDatabase.DepositToSavings(user);
                                        Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal(user)}");
                                        Console.WriteLine();
                                        keepAsking2 = false;
                                    }
                                    else if (accountSelection2 == "C")
                                    {
                                        userDatabase.DepositToChecking(user);
                                        Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal(user)}");
                                        Console.WriteLine();
                                        keepAsking2 = false;
                                    }
                                    else if (counter3 == 3)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Sorry, that isn't a valid input. You have to press S or C.");
                                        Console.WriteLine("Do you want to try again?");
                                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                                        var choice3 = Console.ReadLine().ToUpper();
                                        if (choice3 == "Y")
                                        {
                                            Console.Clear();
                                            continue;
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            keepAsking2 = false;
                                        }
                                        Console.WriteLine();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                                        Console.WriteLine();
                                        counter3 += 1;
                                    }
                                }
                                break;
                            case "W":
                                Console.Clear();
                                Console.WriteLine("");
                                Console.Write("Savings or Checking? (Press C or S then press Enter): ");
                                var accountSelection3 = Console.ReadLine().ToUpper();
                                var keepAsking3 = true;
                                var counter4 = 0;
                                while (keepAsking3)
                                {
                                    if (accountSelection3 == "S")
                                    {
                                        userDatabase.WithdrawFromSavings(user);
                                        Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal(user)}");
                                        Console.WriteLine();
                                        keepAsking3 = false;
                                    }
                                    else if (accountSelection3 == "C")
                                    {
                                        userDatabase.WithdrawFromChecking(user);
                                        Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal(user)}");
                                        Console.WriteLine();
                                        keepAsking3 = false;
                                    }
                                    else if (counter4 == 3)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Sorry, that isn't a valid input. You have to press S or C.");
                                        Console.WriteLine("Do you want to try again?");
                                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                                        var choice4 = Console.ReadLine().ToUpper();
                                        if (choice4 == "Y")
                                        {
                                            Console.Clear();
                                            continue;
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            keepAsking3 = false;
                                        }
                                        Console.WriteLine();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                                        Console.WriteLine();
                                        counter4 += 1;
                                    }
                                }
                                break;
                            case "T": // for transfer function
                                Console.Clear();
                                var keepAsking4 = true;
                                var counter5 = 0;
                                while (keepAsking4)
                                {
                                    Console.WriteLine("");
                                    Console.Write("Transfer (1) from Savings to Checking or (2) from Checking to Savings? (Press 1 or 2 then press Enter): ");
                                    var accountSelection4 = Console.ReadLine().ToUpper();
                                    if (accountSelection4 == "1")
                                    {
                                        userDatabase.TransferFromSavings(user);
                                        Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal(user)}");
                                        Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal(user)}");
                                        Console.WriteLine();
                                        keepAsking4 = false;
                                    }
                                    else if (accountSelection4 == "2")
                                    {
                                        userDatabase.TransferFromChecking(user);
                                        Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal(user)}");
                                        Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal(user)}");
                                        Console.WriteLine();
                                        keepAsking4 = false;
                                    }
                                    else if (counter5 == 3)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Sorry, that isn't a valid input. You have to press 1 or 2.");
                                        Console.WriteLine();
                                        Console.WriteLine("Do you want to try again?");
                                        Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                                        var choice5 = Console.ReadLine().ToUpper();
                                        if (choice5 == "Y")
                                        {
                                            Console.Clear();
                                            continue;
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            keepAsking4 = false;
                                        }
                                        Console.WriteLine();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Sorry, that isn't a valid input. Try again.");
                                        Console.WriteLine();
                                        counter5 += 1;
                                    }
                                }
                                break;
                            case "V":
                                Console.Clear();
                                Console.WriteLine("");
                                userDatabase.GetAllTransactions(user);
                                break;
                            case "Q":
                                Console.Clear();
                                Console.WriteLine("");
                                keepGoing2 = false;
                                break;
                        }
                    }
                }
                Console.WriteLine("Goodbye.");
            }
        }
    }

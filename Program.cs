using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace FirstBankOfSuncoast
{
    class Transaction
    {
        public string TransactionType { get; set; }
        public string Account { get; set; }
        public double Amount { get; set; }
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
        public void DepositToSavings()
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionType = "deposit";
            newTransaction.Account = "savings";
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
        public void DepositToChecking()
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionType = "deposit";
            newTransaction.Account = "checking";
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
        public void WithdrawFromSavings(UserTransactions userDatabase)
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionType = "withdraw";
            newTransaction.Account = "savings";
            var keepAsking = true;
            var counter = 0;
            while (keepAsking)
            {
                Console.WriteLine();
                Console.WriteLine("How much money would you like to withdraw from your savings account? ");
                double amountMoney;
                var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                if (isThisGoodInput && amountMoney > 0 && ((userDatabase.savingsTotal() - amountMoney) >= 0))
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
        public void WithdrawFromChecking(UserTransactions userDatabase)
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionType = "withdraw";
            newTransaction.Account = "checking";
            var keepAsking = true;
            var counter = 0;
            while (keepAsking)
            {
                Console.WriteLine();
                Console.WriteLine("How much money would you like to withdraw from your checking account? ");
                double amountMoney;
                var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                if (isThisGoodInput && amountMoney > 0 && ((userDatabase.checkingTotal() - amountMoney) >= 0))
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
        public void TransferFromChecking()
        {
            var keepAsking = true;
            var counter = 0;
            while (keepAsking)
            {
                Console.WriteLine();
                Console.WriteLine("How much money would you like to transfer from checking to savings? ");
                double amountMoney;
                var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                if (isThisGoodInput && amountMoney > 0 && ((checkingTotal() - amountMoney) >= 0))
                {
                    var newTransaction = new Transaction();
                    newTransaction.TransactionType = "withdraw for transfer";
                    newTransaction.Account = "checking";
                    newTransaction.Amount = amountMoney;
                    transactions.Add(newTransaction);
                    SaveTransactionsToCSV();
                    Console.WriteLine();
                    Console.WriteLine("Funds withdrawn from checking account.");
                    var newTransaction2 = new Transaction();
                    newTransaction2.TransactionType = "deposit for transfer";
                    newTransaction2.Account = "savings";
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
        public void TransferFromSavings()
        {
            var keepAsking = true;
            var counter = 0;
            while (keepAsking)
            {
                Console.WriteLine();
                Console.WriteLine("How much money would you like to transfer from savings to checking? ");
                double amountMoney;
                var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                if (isThisGoodInput && amountMoney > 0 && ((savingsTotal() - amountMoney) >= 0))
                {
                    var newTransaction = new Transaction();
                    newTransaction.TransactionType = "withdraw for transfer";
                    newTransaction.Account = "savings";
                    newTransaction.Amount = amountMoney;
                    transactions.Add(newTransaction);
                    SaveTransactionsToCSV();
                    Console.WriteLine();
                    Console.WriteLine("Funds withdrawn from savings account.");
                    var newTransaction2 = new Transaction();
                    newTransaction2.TransactionType = "deposit for transfer";
                    newTransaction2.Account = "checking";
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
        public void GetAllTransactions()
        {
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction.Description());
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        public double savingsTotal()
        {
            var depositSum = transactions.Where(transaction => transaction.TransactionType == "deposit" && transaction.Account == "savings").Sum(transaction => transaction.Amount);
            var withdrawalSum = transactions.Where(transaction => transaction.TransactionType == "withdraw" && transaction.Account == "savings").Sum(transaction => transaction.Amount);
            var savingsBalance = depositSum - withdrawalSum;
            return savingsBalance;
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        public double checkingTotal()
        {
            var depositSum = transactions.Where(transaction => transaction.TransactionType == "deposit" && transaction.Account == "checking").Sum(transaction => transaction.Amount);
            var withdrawalSum = transactions.Where(transaction => transaction.TransactionType == "withdraw" && transaction.Account == "checking").Sum(transaction => transaction.Amount);
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
            var userDatabase = new UserTransactions();
            userDatabase.LoadTransactionsFromCSV();
            Greeting();
            var keepGoing = true;
            // Console.Write("Please Enter Your Password: ");
            // var password = Console.ReadLine();
            // if ( = "")
            // {

            // }
            // else if (password ==) 
            // {

            // } put a loop in around entire main after this for password protection

            while (keepGoing)
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
                        var counter1 = 0;
                        while (keepAsking1)
                        {
                            if (accountSelection1 == "S")
                            {
                                Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal()}");
                                keepAsking1 = false;
                            }
                            else if (accountSelection1 == "C")
                            {
                                Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal()}");
                                keepAsking1 = false;
                            }
                            else if (counter1 == 3)
                            {
                                Console.Clear();
                                Console.WriteLine("Sorry, that isn't a valid input. You have to press S or C.");
                                Console.WriteLine("Do you want to try again?");
                                Console.Write("(Press Y then press enter for 'Yes'. Press Enter to return to the menu) ");
                                var choice1 = Console.ReadLine().ToUpper();
                                if (choice1 == "Y")
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
                                counter1 += 1;
                            }
                        }
                        break;
                    case "D":
                        Console.Clear();
                        Console.WriteLine("");
                        Console.Write("Savings or Checking? (Press C or S then press Enter): ");
                        var accountSelection2 = Console.ReadLine().ToUpper();
                        var counter2 = 0;
                        var keepAsking2 = true;
                        while (keepAsking2)
                        {
                            if (accountSelection2 == "S")
                            {
                                userDatabase.DepositToSavings();
                                Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal()}");
                                Console.WriteLine();
                                keepAsking2 = false;
                            }
                            else if (accountSelection2 == "C")
                            {
                                userDatabase.DepositToChecking();
                                Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal()}");
                                Console.WriteLine();
                                keepAsking2 = false;
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
                                    keepAsking2 = false;
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
                    case "W":
                        Console.Clear();
                        Console.WriteLine("");
                        Console.Write("Savings or Checking? (Press C or S then press Enter): ");
                        var accountSelection3 = Console.ReadLine().ToUpper();
                        var keepAsking3 = true;
                        var counter3 = 0;
                        while (keepAsking3)
                        {
                            if (accountSelection3 == "S")
                            {
                                userDatabase.WithdrawFromSavings(userDatabase);
                                Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal()}");
                                Console.WriteLine();
                                keepAsking3 = false;
                            }
                            else if (accountSelection3 == "C")
                            {
                                userDatabase.WithdrawFromChecking(userDatabase);
                                Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal()}");
                                Console.WriteLine();
                                keepAsking3 = false;
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
                                    keepAsking3 = false;
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
                    case "T": // for transfer function
                        Console.Clear();
                        var keepAsking4 = true;
                        var counter4 = 0;
                        while (keepAsking4)
                        {
                            Console.WriteLine("");
                            Console.Write("Transfer (1) from Savings to Checking or (2) from Checking to Savings? (Press 1 or 2 then press Enter): ");
                            var accountSelection4 = Console.ReadLine().ToUpper();
                            if (accountSelection4 == "1")
                            {
                                userDatabase.TransferFromSavings();
                                Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal()}");
                                Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal()}");
                                Console.WriteLine();
                                keepAsking4 = false;
                            }
                            else if (accountSelection4 == "2")
                            {
                                userDatabase.TransferFromChecking();
                                Console.WriteLine($"Checking Balance: ${userDatabase.checkingTotal()}");
                                Console.WriteLine($"Savings Balance: ${userDatabase.savingsTotal()}");
                                Console.WriteLine();
                                keepAsking4 = false;
                            }
                            else if (counter4 == 3)
                            {
                                Console.Clear();
                                Console.WriteLine("Sorry, that isn't a valid input. You have to press 1 or 2.");
                                Console.WriteLine();
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
                                    keepAsking4 = false;
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
                    case "V":
                        Console.Clear();
                        Console.WriteLine("");
                        userDatabase.GetAllTransactions();
                        break;
                    case "Q":
                        Console.Clear();
                        Console.WriteLine("");
                        keepGoing = false;
                        break;
                }
            }
        }
    }
}

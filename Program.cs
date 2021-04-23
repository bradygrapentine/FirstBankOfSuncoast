using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

/*

P:

E:

D:

A:

C:

*/

namespace FirstBankOfSuncoast
{
    class Transaction
    {
        public string TransactionType { get; set; }
        public string Account { get; set; }
        public double Amount { get; set; }
        public DateTime TimeOfTransaction { get; set; } = DateTime.Now;
        //--------------------------------------------------------------------------------------------------------------------------------//
        public string Description()
        {
            if (TransactionType == "withdraw")
            {
                return $"{Amount} was withdrawn from {Account} at {TimeOfTransaction}.";
            }
            else
            {
                return $"{Amount} was deposited into {Account} at {TimeOfTransaction}.";
            }
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------//
    class Database
    {
        private List<Transaction> transactions = new List<Transaction>();

        public void LoadTransactionsFromCSV()
        {
            if (File.Exists("transactions.csv"))
            {
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
                Console.WriteLine("How much money would you like to deposit into your savings account? ");
                double amountMoney;
                var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                if (isThisGoodInput)
                {
                    newTransaction.Amount = amountMoney;
                }
                else
                {
                    Console.WriteLine("Sorry, that isn't a valid input, I'm using 0 as your answer.");
                    newTransaction.Amount = 0;
                }
            }
            transactions.Add(newTransaction);
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        public void DepositToChecking(List<Transaction> transactions)
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionType = "deposit";
            newTransaction.Account = "checking";
            var keepAsking = true;
            while (keepAsking)
            {
                Console.WriteLine("How much money would you like to deposit into your checking account? ");
                double amountMoney;
                var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                if (isThisGoodInput)
                {
                    newTransaction.Amount = amountMoney;
                }
                else
                {
                    Console.WriteLine("Sorry, that isn't a valid input, I'm using 0 as your answer.");
                    newTransaction.Amount = 0;
                }
            }
            transactions.Add(newTransaction);
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        public void WithdrawFromSavings()
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionType = "withdraw";
            newTransaction.Account = "savings";
            var keepAsking = true;
            while (keepAsking)
            {
                Console.WriteLine("How much money would you like to withdraw from your savings account? ");
                double amountMoney;
                var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                if (isThisGoodInput)
                {
                    newTransaction.Amount = amountMoney;
                }
                else
                {
                    Console.WriteLine("Sorry, that isn't a valid input, I'm using 0 as your answer.");
                    newTransaction.Amount = 0;
                }
            }
            transactions.Add(newTransaction);
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        public void WithdrawFromChecking()
        {
            var newTransaction = new Transaction();
            newTransaction.TransactionType = "withdraw";
            newTransaction.Account = "checking";
            var keepAsking = true;
            while (keepAsking)
            {
                Console.WriteLine("How much money would you like to withdraw from your checking account? ");
                double amountMoney;
                var isThisGoodInput = Double.TryParse(Console.ReadLine(), out amountMoney);
                if (isThisGoodInput)
                {
                    newTransaction.Amount = amountMoney;
                }
                else
                {
                    Console.WriteLine("Sorry, that isn't a valid input, I'm using 0 as your answer.");
                    newTransaction.Amount = 0;
                }
            }
            transactions.Add(newTransaction);
            transactions.Add(newTransaction);
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
        public void savingsTotal()
        {
            var depositSum = transactions.Where(transaction => transaction.TransactionType == "deposit" && transaction.Account == "savings").Sum(transaction => transaction.Amount);
            var withdrawalSum = transactions.Where(transaction => transaction.TransactionType == "withdraw" && transaction.Account == "savings").Sum(transaction => transaction.Amount);
            var savingsBalance = depositSum - withdrawalSum;
            Console.WriteLine($"Savings Balance: ${savingsBalance}");
        }
        public void checkingTotal()
        {
            var depositSum = transactions.Where(transaction => transaction.TransactionType == "deposit" && transaction.Account == "checking").Sum(transaction => transaction.Amount);
            var withdrawalSum = transactions.Where(transaction => transaction.TransactionType == "withdraw" && transaction.Account == "checking").Sum(transaction => transaction.Amount);
            var checkingBalance = depositSum - withdrawalSum;
            Console.WriteLine($"Checking Balance: ${checkingBalance}");
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
            // Console.WriteLine("(T)ransfer Funds");
            Console.WriteLine("(V)iew Transactions");
            Console.WriteLine("(Q)uit the Application");
            var choice = Console.ReadLine().ToUpper();
            return choice;
        }
        //--------------------------------------------------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------------//
        // static string PromptForString(string prompt)
        // {
        //     Console.Write(prompt);
        //     var userInput = Console.ReadLine();
        //     return userInput;
        // }
        // //--------------------------------------------------------------------------------------------------------------------------------//
        // //--------------------------------------------------------------------------------------------------------------------------------//
        // static double PromptForDouble(string prompt)
        // {
        //     Console.Write(prompt);
        //     double userInput;
        //     var isThisGoodInput = Double.TryParse(Console.ReadLine(), out userInput);
        //     if (isThisGoodInput)
        //     {
        //         return userInput;
        //     }
        //     else
        //     {
        //         Console.WriteLine("Sorry, that isn't a valid input, I'm using 0 as your answer.");
        //         return 0;
        //     }
        // }

        //--------------------------------------------------------------------------------------------------------------------------------//
        // var savings = transactions.Where(transaction => transaction.TransactionType == "savings");
        // var checking = transactions.Where(transaction => transaction.TransactionType == "checking");
        //--------------------------------------------------------------------------------------------------------------------------------//
        static void Main(string[] args)
        {
            Greeting();
            var transactions = new List<Transaction>();
            var keepGoing = true;
            while (keepGoing)
            {
                var choice = Menu();
                switch (choice)
                {
                    case "C":
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Press S then press Enter to display savings account balance.");
                        Console.WriteLine("Press C then press Enter to display checking account balance.");
                        var accountSelection = Console.ReadLine().ToUpper();
                        if (accountSelection == "S")
                        {
                            Console.WriteLine(transactions.Where(transaction => transaction.Account == "savings"));
                        }
                        else if (accountSelection == "C")
                        {

                            Console.WriteLine(transactions.Where(transaction => transaction.Account == "checking"));
                        }

                        break;
                    case "D":
                        Console.Clear();
                        Console.WriteLine("");
                        break;
                    case "W":
                        Console.Clear();
                        Console.WriteLine("");
                        break;
                    case "T":
                        Console.Clear();
                        Console.WriteLine();
                        break;
                    case "V":
                        Console.Clear();
                        Console.WriteLine("");
                        break;
                    case "Q":
                        Console.Clear();
                        Console.WriteLine("");
                        keepGoing = false;
                        break;
                }
            }
            Console.Clear();
            Console.WriteLine("Closing First Bank of Suncoast Account Management Application");
        }
    }

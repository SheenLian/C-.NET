using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadifyBank.Interfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            //initializing..
            IAccounts sheen1 = new IAccounts();
            sheen1.accnum = "SV-000123";
            sheen1.bal = 50000m;
            sheen1.cname = "Sheen Lian";
            sheen1.opday = new DateTime(2017, 1, 1);

            IAccounts sheen2 = new IAccounts();
            sheen2.accnum = "LN-000103";
            sheen2.bal = 50000m;
            sheen2.cname = "Sheen Lian";
            sheen2.opday = new DateTime(2017, 1, 1);

            IAccounts lily = new IAccounts();
            lily.accnum = "LN-000121";
            lily.bal = 50000m;
            lily.cname = "Lily Shi";
            lily.opday = new DateTime(2017, 1, 1);

            statementrow statement1 = new statementrow();
            statementrow statement2 = new statementrow();
            statementrow statement3 = new statementrow();
            IReadifybank banking = new IReadifybank();

            //add two accounts into bank list
            banking.AccountList.Add(lily);
            banking.AccountList.Add(sheen1);

            //test calculate interest rate
            DateTime todate = new DateTime(2020, 1, 1);
            banking.CalculateInterestToDate(sheen1, todate);

            //transaction 1: deposit 10000 dollars into sheen1
            Console.WriteLine(">>>Deposit<<<");
            statement1.account = sheen1;
            statement1.tran_amount = 10000m;
            statement1.description = "Save for good use!";
            statement1.tran_date = DateTime.Now;
            banking.PerformDeposit(statement1.account, statement1.tran_amount, statement1.description, statement1.tran_date);
            banking.TransactionLog.Add(statement1);

            //transaction 2: withdrawal 10000 dollars from lily after some time
            TimeSpan duration1 = new TimeSpan(888, 88, 88);
            Console.WriteLine(">>>Withdrawal<<<");
            statement2.account = lily;
            statement2.tran_amount = 10000m;
            statement2.description = "Withdrawal for good use!";
            statement2.tran_date = DateTime.Now.Add(duration1);
            banking.PerformWithdrawal(statement2.account, statement2.tran_amount, statement2.description, statement2.tran_date);
            banking.TransactionLog.Add(statement2);

            //transaction 3: transfer 10000 dollars from sheen1 to lily after some time 
            // sheen1 and lily are different type of accounts so this will not work
            TimeSpan duration2 = new TimeSpan(666, 66, 66);
            Console.WriteLine(">>>Transfer<<<");
            statement3.account = sheen1;
            statement3.tran_amount = 10000m;
            statement3.description = "Giving is loving!";
            statement3.tran_date = DateTime.Now.Add(duration2);
            banking.PerformTransfer(statement3.account, lily, statement3.tran_amount, statement3.description, statement3.tran_date);


            //now try again from sheen2
            statement3.account = sheen2;
            banking.PerformTransfer(statement3.account, lily, statement3.tran_amount, statement3.description, statement3.tran_date);
            banking.TransactionLog.Add(statement3);


        }
    }

    //IAccount Interface ~~~~~~~~~~~~~~~~~~~~~
    public interface IAccount
    {
        
        DateTimeOffset OpenedDate { get; }
        string CustomerName { get; }
        string AccountNumber { get; }
        decimal Balance { get; }
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Inheriting class for IAccount~~~~~~~~~~~
    public class IAccounts : IAccount
    {
        public string cname;
        public string accnum;
        public decimal bal;
        public DateTimeOffset opday;
        //contructor for accounts
 
        
        //get customer name
        public string CustomerName{
            get
            {
                return cname;
            }
        }
        //get account number
        public string AccountNumber
        {
            get
            {
                return accnum;
            }
        }
        //get balance
        public decimal Balance
        {
            get
            {
                return bal;
            }
        }
        //get open date
        public DateTimeOffset OpenedDate
        {
            get
            {
                return opday;
            }
        }
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //IStatementRow Interface~~~~~~~~~~~~~~~~~
    public interface IStatementRow
    {
        IAccount Account { get; }
        DateTimeOffset Date { get; }
        decimal Amount { get; }
        decimal Balance { get; }
        string Description { get; }
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Inheriting class for IStatementRow~~~~~~
    public class statementrow : IStatementRow
    {
        public decimal tran_amount;
        public decimal aftertran_balance;
        public DateTimeOffset tran_date;
        public string description;
        public IAccounts account;

        public IAccounts Account
        {
            get
            {
                return account;
            }
        }

        public decimal Amount
        {
            get
            {
                return tran_amount;
            }
        }

        public decimal Balance
        {
            get
            {
                return account.bal-tran_amount;
            }
        }

        public DateTimeOffset Date
        {
            get
            {
                return tran_date;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        IAccount IStatementRow.Account
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }







    public interface IReadifyBank
    {
        
        List<IAccounts> AccountList { get; }
        List<statementrow> TransactionLog { get; }
        IAccount OpenHomeLoanAccount(string customerName, DateTimeOffset openDate);
        IAccount OpenSavingsAccount(string customerName, DateTimeOffset openDate);
        void PerformDeposit(IAccounts account, decimal amount, string description, DateTimeOffset depositDate);
        void PerformWithdrawal(IAccounts account, decimal amount, string description, DateTimeOffset withdrawalDate);
        void PerformTransfer(IAccounts from, IAccounts to, decimal amount, string description, DateTimeOffset transferDate);     
        decimal GetBalance(IAccounts account);
        decimal CalculateInterestToDate(IAccounts account, DateTimeOffset toDate);

        /// <summary>
        /// Get mini statement (the last 5 transactions occurred on an account)
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <returns>Last five transactions</returns>
        IEnumerable<statementrow> GetMiniStatement(IAccounts account);

        /// <summary>
        /// Close an account
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <param name="closeDate">Close Date</param>
        /// <returns>All transactions happened on the closed account</returns>
        IEnumerable<statementrow> CloseAccount(IAccounts account, DateTimeOffset closeDate);
    }

    public class IReadifybank : IReadifyBank
    {
        List<IAccounts> accountList = new List<IAccounts> { };
        List<statementrow> statementList = new List<statementrow> { };

        // account list
        public List<IAccounts> AccountList
        {
            get
            {
                return accountList;
            }
        }
        // transaction list
        public List<statementrow> TransactionLog
        {
            get
            {
                return statementList;
            }
        }
        // calculate rate
        public decimal CalculateInterestToDate(IAccounts account, DateTimeOffset toDate)
        {
            double addvalue;
            double year_diff, month_diff;
            double origin = (double)account.Balance;
            double money = origin;
            if (account.AccountNumber.Contains("LN"))
            {
                year_diff = toDate.Subtract(account.OpenedDate).TotalDays / 365;
                for (int i = 0; i < Math.Round(year_diff); i++) 
                    {
                    money = money * (1 + 0.0399);
                    }
                addvalue = money - origin;
            }
            else
            {
                month_diff = toDate.Subtract(account.OpenedDate).TotalDays / 30;
                for (int i = 0; i < Math.Round(month_diff); i++)
                {
                    money = money * (1 + 0.06);
                }
                addvalue = money - origin;
            }
            Console.WriteLine("*******************************");
            Console.WriteLine();
            Console.WriteLine(">Your interest rate is: "+ addvalue);
            Console.WriteLine();
            Console.WriteLine("*******************************");
            return (decimal)addvalue;
        }

        // check balace of the account
        public decimal GetBalance(IAccounts account)
        {
            return account.Balance;
        }

        // open a home loan account
        public IAccount OpenHomeLoanAccount(string customerName, DateTimeOffset openDate)
        {
            IAccounts openedaccount = new IAccounts();
            foreach (IAccounts x in accountList) {
                if (x.CustomerName == customerName) {
                    openedaccount = x;
                }
            }
            Console.WriteLine(">Openning account" + "customeName");
            Console.WriteLine(">Account Number: " + openedaccount.AccountNumber);
            Console.WriteLine(">Balance: " + openedaccount.Balance);
            Console.WriteLine(">Open date: " + openDate);
            return openedaccount;

        }

        // open a saving account
        public IAccount OpenSavingsAccount(string customerName, DateTimeOffset openDate)
        {
            IAccounts openedaccount = new IAccounts();
            foreach (IAccounts x in accountList)
            {
                if (x.CustomerName == customerName)
                {
                    openedaccount = x;
                }
            }
            
            Console.WriteLine();
            Console.WriteLine(">Openning account: " + customerName);
            Console.WriteLine(">Account Number: " + openedaccount.AccountNumber);
            Console.WriteLine(">Balance: " + openedaccount.Balance);
            Console.WriteLine(">Open date: " + openDate);
            Console.WriteLine();
            Console.WriteLine("*******************************");
            return openedaccount;
        }

        //deposit
        public void PerformDeposit(IAccounts account, decimal amount, string description, DateTimeOffset depositDate)
        {

            
            account.bal += amount;
            
            Console.WriteLine();
            Console.WriteLine(">Depositing " + amount + " into " + account.AccountNumber);
            Console.WriteLine(">Transaction date " + depositDate);
            Console.WriteLine(">Message: " + description);
            Console.WriteLine();
            Console.WriteLine("*******************************");
        }
        //transfer
        public void PerformTransfer(IAccounts from, IAccounts to, decimal amount, string description, DateTimeOffset transferDate)
        {
            if (from.AccountNumber.Substring(0, 2) == to.AccountNumber.Substring(0, 2))
            {
                from.bal -= amount;
                to.bal += amount;
                
                Console.WriteLine();
                Console.WriteLine(">Transfering " + amount + " from " + from.AccountNumber + " to " + to.AccountNumber);
                Console.WriteLine(">Transaction date " + transferDate);
                Console.WriteLine(">Message: " + description);
                Console.WriteLine();
                Console.WriteLine("*******************************");
            }// prevent users transfer money to a diff type of account (SV to LN)
            else {
                
                Console.WriteLine();             
                Console.WriteLine(">Transaction failed: You can't tranfer to a different type of account!");      
                Console.WriteLine();
                Console.WriteLine("*******************************");
            }
        }
        //withdrawal
        public void PerformWithdrawal(IAccounts account, decimal amount, string description, DateTimeOffset withdrawalDate)
        {
            account.bal -= amount;
            
            Console.WriteLine();
            Console.WriteLine(">Withdrawaling " + amount + " into " + account.AccountNumber);
            Console.WriteLine(">Transaction date " + withdrawalDate);
            Console.WriteLine(">Message: " + description);
            Console.WriteLine();
            Console.WriteLine("*******************************");
        }

        public IEnumerable<statementrow> CloseAccount(IAccounts account, DateTimeOffset closeDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<statementrow> GetMiniStatement(IAccounts account)
        {
            throw new NotImplementedException();
        }
    }
}


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
            
            // initializing a IReadfiybank object
            IReadifybank banking = new IReadifybank();

            //open 3 account on 1/1/2017, 1 LN account and 2 SV accounts
            Console.WriteLine("~~~~~~~~~~~~OPEN ACCOUNTS~~~~~~~~~~~\n");
            DateTime open_day = new DateTime(2017, 1, 1);
            IAccounts sheen_LN = banking.OpenHomeLoanAccount("Sheen Lian", open_day);
            IAccounts sheen_SV = banking.OpenSavingsAccount("Sheen Lian", open_day);
            IAccounts lily_SV = banking.OpenSavingsAccount("Lily Shi", open_day);
            Console.WriteLine("\n\n");

            //transaction 1: deposit 50000 dollars into each of these accounts
            Console.WriteLine("~~~~~~~~~~~~DEPOSIR~~~~~~~~~~~~~~~~~\n");
            banking.PerformDeposit(sheen_SV, 50000m, "Save for good use!", DateTime.Now);
            banking.PerformDeposit(sheen_LN, 50000m, "Save for good use!", DateTime.Now);
            banking.PerformDeposit(lily_SV, 50000m, "Save for good use!", DateTime.Now);
            Console.WriteLine("\n\n");
           
            //check balance of thse accounts
            Console.WriteLine("~~~~~~~~~~~~Chec balance~~~~~~~~~~~~\n");
            banking.GetBalance(sheen_LN);
            banking.GetBalance(sheen_SV);
            banking.GetBalance(lily_SV);
            Console.WriteLine("\n\n");

            //test calculate interest rate
            Console.WriteLine("~~~~~~~~Calculate interest rate~~~~~~");
            DateTime todate = new DateTime(2020, 1, 1);
            banking.CalculateInterestToDate(sheen_LN, todate);
            Console.WriteLine("\n\n");
            

            //transaction 2: withdrawal 10000 dollars from lily_SV after some time
            TimeSpan duration1 = new TimeSpan(888, 88, 88);
            Console.WriteLine("~~~~~~~~~~~~Withdrawal~~~~~~~~~~~~~~~");
            banking.PerformWithdrawal(lily_SV, 10000m, "Withdrawal for good use!", DateTime.Now.Add(duration1));
            Console.WriteLine("\n\n");


            //transaction 3: transfer 6666 dollars from sheen1 to lily after some time  
            TimeSpan duration2 = new TimeSpan(666, 66, 66);
            Console.WriteLine("~~~~~~~~~~~~~~~~Transfer~~~~~~~~~~~~~");
            //THIS WILL NOT WORK AND SHOW WARNING!!!:because sheen1 and lily are different type of accounts 
            //Just test the warning giving part of the code:
            banking.PerformTransfer(sheen_LN, lily_SV, 6666m, "Giving is loving!", DateTime.Now.Add(duration2));
            //now try again from sheen2
            banking.PerformTransfer(sheen_SV, lily_SV, 6666m, "Giving is loving!", DateTime.Now.Add(duration2));
            Console.WriteLine("\n\n");

            //Do some different operation on sheen2 
            ///<summary>
            ///this part use multithreading to do transactions
            ///so the output in application console will be messy
            ///if you want to see the neat version of this, plz unquote the code below which 
            ///are actully doing the same thing without applying thread
            Task tran1 = new Task(() => banking.PerformDeposit(sheen_SV, 10000m, "Save for good use!", DateTime.Now));
            Task tran2 = new Task(() => banking.PerformDeposit(sheen_SV, 11111m, "Save for good use!", DateTime.Now));
            Task tran3 = new Task(() => banking.PerformDeposit(sheen_SV, 12222m, "Save for good use!", DateTime.Now));
            Task tran4 = new Task(() => banking.PerformDeposit(sheen_SV, 111.11m, "Save for good use!", DateTime.Now));
            Task tran5 = new Task(() => banking.PerformTransfer(sheen_SV, lily_SV, 222.22m, "Giving is loving!", DateTime.Now.Add(duration2)));
            Task tran6 = new Task(() => banking.PerformWithdrawal(sheen_SV, 333.33m, "Withdrawal for good use!", DateTime.Now.Add(duration1)));
            Task tran7 = new Task(() => banking.PerformDeposit(sheen_SV, 444.44m, "Save for good use!", DateTime.Now));
            Task tran8 = new Task(() => banking.PerformTransfer(sheen_SV, lily_SV, 555.55m, "Giving is loving!", DateTime.Now.Add(duration2)));
            tran1.Start();
            tran2.Start();
            tran3.Start();
            tran4.Start();
            tran5.Start();
            tran6.Start();
            tran7.Start();
            tran8.Start();
            //neat version
            /*
            banking.PerformDeposit(sheen_SV, 10000m, "Save for good use!", DateTime.Now);
            banking.PerformDeposit(sheen_SV, 10001m, "Save for good use!", DateTime.Now);
            banking.PerformDeposit(sheen_SV, 10002m, "Save for good use!", DateTime.Now);
            banking.PerformDeposit(sheen_SV, 111.11m, "Save for good use!", DateTime.Now);
            banking.PerformTransfer(sheen_SV, lily_SV, 222.22m, "Giving is loving!", DateTime.Now.Add(duration2));
            banking.PerformWithdrawal(sheen_SV, 333.33m, "Withdrawal for good use!", DateTime.Now.Add(duration1));
            banking.PerformDeposit(sheen_SV, 444.44m, "Save for good use!", DateTime.Now);
            banking.PerformTransfer(sheen_SV, lily_SV, 555.55m, "Giving is loving!", DateTime.Now.Add(duration2));
            */


            

            //try get last five transaction
            IEnumerable<statementrow> a = banking.GetMiniStatement(sheen_SV);
            Console.WriteLine("~~~~~~~~Get last five~~~~~~~~~~");
            foreach (var x in a) {
                Console.WriteLine(x.Amount);
            }
            Console.WriteLine("\n\n");
            //try close transaction
            IEnumerable<statementrow> b = banking.CloseAccount(sheen_SV,DateTime.Now);
            Console.WriteLine("~~~~~~~Close account~~~~~~~~~~~");
            foreach (var x in b)
            {
                Console.WriteLine(x.Amount);
            }
            Console.WriteLine("\n\n");
            //*****FOR THIS TRY GET TRANSACATION PART, SINCE THE RETURN TYPE IS A ENUMERABLE<IStatmentRow> TYPE,
            //*****SO TO PROVE IT IS WORKING, I PRINT OUT THE AMOUNT OF EACH TRANSACTION
            //time order issue explained in the method define



        }
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



    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Inheriting class for IReadifyBank~~~~~~~
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
            double year_diff = 0, month_diff;
            double origin = (double)account.Balance;
            double money = origin;
            if (account.AccountNumber.Contains("LN"))
            {
                year_diff = toDate.Subtract(account.OpenedDate).TotalDays/365.00;
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
            Console.WriteLine(year_diff);
            Console.WriteLine(">Your interest rate is: " + addvalue);
            Console.WriteLine();
            Console.WriteLine("*******************************");
            return (decimal)addvalue;
        }

        // check balace of the account
        public decimal GetBalance(IAccounts account)
        {
            Console.WriteLine(account.AccountNumber + " => Bal : " + account.Balance+"\n");
            return account.Balance;
        }

        // open a home loan account
        public IAccounts OpenHomeLoanAccount(string customerName, DateTimeOffset openDate)
        {
            IAccounts openedaccount = new IAccounts();
            openedaccount.cname = customerName;
            openedaccount.opday = openDate;
            Random rd = new Random();
            bool Num_taken = true;
            int counter = 0;
            while (Num_taken)
            {
                int r = rd.Next(1, 999999);
                openedaccount.accnum = "LN-" + r.ToString("D6");
                for (counter =0; counter < accountList.Count; counter++)
                {
                    
                    if (accountList[counter].AccountNumber == openedaccount.AccountNumber)
                    {
                        
                        break;
                    }
                }
                if (counter == accountList.Count)
                {
                    Num_taken = false;
                }
                
            }


            Console.WriteLine();
            Console.WriteLine(">Openning account: " + customerName);
            Console.WriteLine(">Account Number: " + openedaccount.AccountNumber);
            Console.WriteLine(">Open date: " + openDate);
            Console.WriteLine();
            Console.WriteLine("*******************************");

            this.AccountList.Add(openedaccount);
            
            return openedaccount;

        }

        // open a saving account
        public IAccounts OpenSavingsAccount(string customerName, DateTimeOffset openDate)
        {
            IAccounts openedaccount = new IAccounts();
            openedaccount.cname = customerName;
            openedaccount.opday = openDate;
            Random rd = new Random();
            bool Num_taken = true;
            int counter;
            while (Num_taken)
            {
                int r = rd.Next(1, 999999);
                openedaccount.accnum = "SV-" + r.ToString("D6");
                for (counter = 0; counter < accountList.Count; counter++)
                {
                    if (accountList[counter].AccountNumber == openedaccount.AccountNumber)
                    {
                        break;
                    }
                }
                if (counter == accountList.Count)
                {
                    Num_taken = false;
                }

            }


            Console.WriteLine();
            Console.WriteLine(">Openning account: " + customerName);
            Console.WriteLine(">Account Number: " + openedaccount.AccountNumber);
            Console.WriteLine(">Open date: " + openDate);
            Console.WriteLine();
            Console.WriteLine("*******************************");
            this.AccountList.Add(openedaccount);
            return openedaccount;
        }


        //deposit
        public void PerformDeposit(IAccounts account, decimal amount, string description, DateTimeOffset depositDate)
        {
            //adding money into account
            account.bal += amount;
            //creating a new transaction log
            statementrow statement = new statementrow();
            statement.account = account;
            statement.tran_amount = amount;
            statement.tran_date = depositDate;
            statement.description = description;
            statement.aftertran_balance = account.Balance;
            this.TransactionLog.Add(statement);
            //print outout
            Console.WriteLine();
            Console.WriteLine(">Depositing " + amount + " into " + account.AccountNumber);
            Console.WriteLine(">Transaction date " + depositDate);
            Console.WriteLine(">Message: " + description);
            Console.WriteLine(">Balance after transaction: " + this.GetBalance(account));
            Console.WriteLine();
            Console.WriteLine("*******************************");
        }


        //transfer
        public void PerformTransfer(IAccounts from, IAccounts to, decimal amount, string description, DateTimeOffset transferDate)
        {
            if (from.AccountNumber.Substring(0, 2) == to.AccountNumber.Substring(0, 2))
            {
                // substract money from sender
                from.bal -= amount;
                // add money to reciever
                to.bal += amount;

                //creating trasaction logs: there is one transaction for each accout, so in total two logs needed
                // log for sender
                statementrow statement1 = new statementrow();
                statement1.account = from;
                statement1.tran_amount = amount;
                statement1.tran_date = transferDate;
                statement1.description = description;
                statement1.aftertran_balance = from.Balance;
                this.TransactionLog.Add(statement1);
                // log for reciever
                statementrow statement2 = new statementrow();
                statement2.account = to;
                statement2.tran_amount = amount;
                statement2.tran_date = transferDate;
                statement2.description = description;
                statement2.aftertran_balance = to.Balance;
                this.TransactionLog.Add(statement2);
                //print outout
                Console.WriteLine();
                Console.WriteLine(">Transfering " + amount + " from " + from.AccountNumber + " to " + to.AccountNumber);
                Console.WriteLine(">Transaction date " + transferDate);
                Console.WriteLine(">Message: " + description);
                Console.WriteLine();
                Console.WriteLine("*******************************");
            }// prevent users transfer money to a diff type of account (SV to LN)
            else
            {

                Console.WriteLine();
                Console.WriteLine(">Transaction failed: You can't tranfer to a different type of account!");
                Console.WriteLine();
                Console.WriteLine("*******************************");
            }
        }


        //withdrawal
        public void PerformWithdrawal(IAccounts account, decimal amount, string description, DateTimeOffset withdrawalDate)
        {
            // substact money from account
            account.bal -= amount;
            //creating a new transcaction log
            statementrow statement = new statementrow();
            statement.account = account;
            statement.tran_amount = amount;
            statement.tran_date = withdrawalDate;
            statement.description = description;
            statement.aftertran_balance = account.Balance;
            this.TransactionLog.Add(statement);
            //print output
            Console.WriteLine();
            Console.WriteLine(">Withdrawaling " + amount + " from " + account.AccountNumber);
            Console.WriteLine(">Transaction date " + withdrawalDate);
            Console.WriteLine(">Message: " + description);
            Console.WriteLine(">Balance after transaction: " + this.GetBalance(account));
            Console.WriteLine();
            Console.WriteLine("*******************************");
        }

        //All transaction logs for the given acount
        public IEnumerable<statementrow> CloseAccount(IAccounts account, DateTimeOffset closeDate)
        {

            foreach (var x in this.TransactionLog)
            {
                if (x.account == account)
                {
                    yield return x;
                }
            }
        }

        //last five transacation logs for the given account
        public IEnumerable<statementrow> GetMiniStatement(IAccounts account)
        {
            List<statementrow> took = new List<statementrow> { };
            foreach (var x in this.TransactionLog)
            {
                if (x.account == account)
                {
                    took.Add(x);
                }
            }
            //**Here the transactions were made in time order and the GetMiniStatment operates in the same order
            //**so what we need is just reverse the for loop
            for (int i = took.Count; i > took.Count - 5; i--)
            {
                statementrow a = took[i - 1];
                yield return a;
            }
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

    //IStatementRow Interface~~~~~~~~~~~~~~~~~
    public interface IStatementRow
    {
        IAccount Account { get; }
        DateTimeOffset Date { get; }
        decimal Amount { get; }
        decimal Balance { get; }
        string Description { get; }
    }

    //IReadifyBank Interface~~~~~~~~~~~~~~~~~~
    public interface IReadifyBank
    {
        List<IAccounts> AccountList { get; }
        List<statementrow> TransactionLog { get; }
        IAccounts OpenHomeLoanAccount(string customerName, DateTimeOffset openDate);
        IAccounts OpenSavingsAccount(string customerName, DateTimeOffset openDate);
        void PerformDeposit(IAccounts account, decimal amount, string description, DateTimeOffset depositDate);
        void PerformWithdrawal(IAccounts account, decimal amount, string description, DateTimeOffset withdrawalDate);
        void PerformTransfer(IAccounts from, IAccounts to, decimal amount, string description, DateTimeOffset transferDate);
        decimal GetBalance(IAccounts account);
        decimal CalculateInterestToDate(IAccounts account, DateTimeOffset toDate);
        IEnumerable<statementrow> GetMiniStatement(IAccounts account);
        IEnumerable<statementrow> CloseAccount(IAccounts account, DateTimeOffset closeDate);
    }

}



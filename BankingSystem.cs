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

            //initializing..3 accounts 1 LN and 2 SV
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



            // initializing a IReadfiybank object
            IReadifybank banking = new IReadifybank();

            //add 3 accounts into bank list
            banking.AccountList.Add(lily);
            banking.AccountList.Add(sheen1);
            banking.AccountList.Add(sheen2);


            //test calculate interest rate
            DateTime todate = new DateTime(2020, 1, 1);
            banking.CalculateInterestToDate(sheen1, todate);

            //test open account
            banking.OpenSavingsAccount("Lily Shi", DateTime.Now);//Try to open a not exist account, will show error
            banking.OpenHomeLoanAccount("Sheen Lian", DateTime.Now);//Open a exist account


            //transaction 1: deposit 10000 dollars into sheen1
            Console.WriteLine(">>>Deposit<<<");
            banking.PerformDeposit(sheen1, 10000m, "Save for good use!", DateTime.Now);
            
            //transaction 2: withdrawal 10000 dollars from lily after some time
            TimeSpan duration1 = new TimeSpan(888, 88, 88);
            Console.WriteLine(">>>Withdrawal<<<");
            banking.PerformWithdrawal(lily, 10000m, "Withdrawal for good use!", DateTime.Now.Add(duration1));
            

            //transaction 3: transfer 6666 dollars from sheen1 to lily after some time 

            //THIS WILL NOT WORK AND SHOW WARNING!!!:because sheen1 and lily are different type of accounts 
            //Just test the warning giving fart of the code:
            TimeSpan duration2 = new TimeSpan(666, 66, 66);
            Console.WriteLine(">>>Transfer<<<");
            banking.PerformTransfer(sheen1, lily, 6666m, "Giving is loving!", DateTime.Now.Add(duration2));
            //now try again from sheen2
            banking.PerformTransfer(sheen2, lily, 6666m, "Giving is loving!", DateTime.Now.Add(duration2));

            //Do some different operation on sheen2 
            ///<summary>
            ///this part use multithreading to do transactions
            ///so the output in application console will be messy
            ///if you want to see the neat version of this, plz unquote the code below which 
            ///are actully doing the same thing without applying thread
            Task tran1 = new Task(() => banking.PerformDeposit(sheen2, 10000m, "Save for good use!", DateTime.Now));
            Task tran2 = new Task(() => banking.PerformDeposit(sheen2, 11111m, "Save for good use!", DateTime.Now));
            Task tran3 = new Task(() => banking.PerformDeposit(sheen2, 12222m, "Save for good use!", DateTime.Now));
            Task tran4 = new Task(() => banking.PerformDeposit(sheen2, 111.11m, "Save for good use!", DateTime.Now));
            Task tran5 = new Task(() => banking.PerformTransfer(sheen2, lily, 222.22m, "Giving is loving!", DateTime.Now.Add(duration2)));
            Task tran6 = new Task(() => banking.PerformWithdrawal(sheen2, 333.33m, "Withdrawal for good use!", DateTime.Now.Add(duration1)));
            Task tran7 = new Task(() => banking.PerformDeposit(sheen2, 444.44m, "Save for good use!", DateTime.Now));
            Task tran8 = new Task(() => banking.PerformTransfer(sheen2, lily, 555.55m, "Giving is loving!", DateTime.Now.Add(duration2)));
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
            banking.PerformDeposit(sheen2, 10000m, "Save for good use!", DateTime.Now);
            banking.PerformDeposit(sheen2, 10001m, "Save for good use!", DateTime.Now);
            banking.PerformDeposit(sheen2, 10002m, "Save for good use!", DateTime.Now);
            banking.PerformDeposit(sheen2, 111.11m, "Save for good use!", DateTime.Now);
            banking.PerformTransfer(sheen2, lily, 222.22m, "Giving is loving!", DateTime.Now.Add(duration2));
            banking.PerformWithdrawal(sheen2, 333.33m, "Withdrawal for good use!", DateTime.Now.Add(duration1));
            banking.PerformDeposit(sheen2, 444.44m, "Save for good use!", DateTime.Now);
            banking.PerformTransfer(sheen2, lily, 555.55m, "Giving is loving!", DateTime.Now.Add(duration2));
            */


            //try get last five transaction
            
            IEnumerable<statementrow> a = banking.GetMiniStatement(sheen2);
            Console.WriteLine("~~~~Get last five~~~~~~");
            foreach (var x in a) {
                Console.WriteLine(x.Amount);
            }
            //try close transaction
            IEnumerable<statementrow> b = banking.CloseAccount(sheen2,DateTime.Now);
            Console.WriteLine("~~~~Close account~~~~~~");
            foreach (var x in b)
            {
                Console.WriteLine(x.Amount);
            }
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
            Console.WriteLine(">Your interest rate is: " + addvalue);
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
            bool exist = false; // test flag for existance of account
            foreach (IAccounts x in accountList)
            {
                // make sure open the LN account under same name
                if (x.CustomerName == customerName && x.AccountNumber.Contains("LN"))
                {
                    openedaccount = x;
                    exist = true;//set to true if account exists
                }
            }
            if (exist)
            {
                Console.WriteLine();
                Console.WriteLine(">Openning account: " + customerName);
                Console.WriteLine(">Account Number: " + openedaccount.AccountNumber);
                Console.WriteLine(">Balance: " + openedaccount.Balance);
                Console.WriteLine(">Open date: " + openDate);
                Console.WriteLine();
                Console.WriteLine("*******************************");
            }
            else
            {
                Console.WriteLine(">Error: Account does not exist!");
                Console.WriteLine();
                Console.WriteLine("*******************************");
            }
            return openedaccount;

        }

        // open a saving account
        public IAccount OpenSavingsAccount(string customerName, DateTimeOffset openDate)
        {
            IAccounts openedaccount = new IAccounts();
            bool exist = false; // test flag for existance of account
            foreach (IAccounts x in accountList)
            {
                if (x.CustomerName == customerName && x.AccountNumber.Contains("SV"))
                {
                    openedaccount = x;
                    exist = true;//set to true if account exists
                }
            }

            if (exist)
            {
                Console.WriteLine();
                Console.WriteLine(">Openning account: " + customerName);
                Console.WriteLine(">Account Number: " + openedaccount.AccountNumber);
                Console.WriteLine(">Balance: " + openedaccount.Balance);
                Console.WriteLine(">Open date: " + openDate);
                Console.WriteLine();
                Console.WriteLine("*******************************");
            }
            else
            {
                Console.WriteLine(">Error: Account does not exist!");
                Console.WriteLine();
                Console.WriteLine("*******************************");
            }

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
        IAccount OpenHomeLoanAccount(string customerName, DateTimeOffset openDate);
        IAccount OpenSavingsAccount(string customerName, DateTimeOffset openDate);
        void PerformDeposit(IAccounts account, decimal amount, string description, DateTimeOffset depositDate);
        void PerformWithdrawal(IAccounts account, decimal amount, string description, DateTimeOffset withdrawalDate);
        void PerformTransfer(IAccounts from, IAccounts to, decimal amount, string description, DateTimeOffset transferDate);
        decimal GetBalance(IAccounts account);
        decimal CalculateInterestToDate(IAccounts account, DateTimeOffset toDate);
        IEnumerable<statementrow> GetMiniStatement(IAccounts account);
        IEnumerable<statementrow> CloseAccount(IAccounts account, DateTimeOffset closeDate);
    }

}



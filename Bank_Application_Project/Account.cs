using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application_Project
{
    public abstract class Account
    {
        protected static int accountNrIncrememnt = 1000;
        protected decimal _balance;
        protected decimal _interestRate;
        protected string _accountType;
        protected int _accountNumber;
        protected List<Transactions> _transactionList = new List<Transactions>();
        public int AccountNumber { get => _accountNumber; }
        public decimal Balance { get => _balance; }
        public decimal InterestRate { get => _interestRate; }
        public string AccountType { get => _accountType; }
        public List<Transactions> TransactionList { get => _transactionList; }

        public override string ToString()
        {
            string accountInfo = $"{AccountNumber} | {_accountType} | Balance: ${Balance} | Interest Rate: {InterestRate * 100 - 100}% ";
            return accountInfo;
        }
        public virtual bool Deposit(decimal inputCash)
        {
            try
            {
                if (inputCash <= 0) { return false; }
                else
                {
                    _balance += inputCash;
                    return true;
                }
            }
            catch { return false; }
        }
        public abstract bool Withdraw(decimal inputCash);
    }
}

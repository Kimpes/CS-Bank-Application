using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application_Project
{
    public class Transactions
    {
        private int _accountNumber;
        private DateTime _currentTime;
        private string _transactionType;
        private decimal _amount;
        private decimal _balance;

        public Transactions(int accNumber, string transType, decimal amount, decimal balance)
        {
            _accountNumber = accNumber;
            _transactionType = transType;
            _amount = amount;
            _balance = balance;
            _currentTime = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{_currentTime} | {_transactionType} : ${_amount} | Balance: ${_balance}";
        }
    }
}

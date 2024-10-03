using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application_Project
{
    class CreditAccount : Account
    {
        private decimal _debtInterest;
        private long _creditLimit;
        public decimal DebtInterest { get => _debtInterest; }

        public CreditAccount()
        {
            accountNrIncrememnt++;
            _balance = 0;
            _interestRate = 1.005M;
            _debtInterest = 1.07M;
            _creditLimit = 5000;
            _accountType = "Credit Account";
            _accountNumber = accountNrIncrememnt;
        }

        public override bool Withdraw(decimal inputCash)
        {
            try
            {
                if (inputCash <= 0 || inputCash > Balance + _creditLimit) { return false; }
                else
                {
                    _balance -= inputCash;
                    return true;
                }
            }
            catch { return false; }
        }

        public override string ToString()  //we first call the base class's ToString and then add everything extra on top of it
        {
            string accountInfo = base.ToString();
            accountInfo += $"| Debt Interest Rate: {_debtInterest * 100 - 100}% | Credit Limit: {_creditLimit}";
            return accountInfo;
        }
    }
}

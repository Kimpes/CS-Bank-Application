using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application_Project
{
    public class SavingsAccount : Account
    {

        public SavingsAccount()
        {
            accountNrIncrememnt++;
            _balance = 0;
            _interestRate = 1.01M;
            _accountType = "Savings Account";
            _accountNumber = accountNrIncrememnt;
        }
        
        public override bool Withdraw(decimal inputCash)
        {
            try
            {
                if (inputCash <= 0 || inputCash > Balance) { return false; }
                else
                {
                    _balance -= inputCash;
                    return true;
                }
            }
            catch { return false; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application_Project
{
    public class Customer
    {
        private string _customerName;
        private long _sSN;
        private List<Account> _accounts = new List<Account>();
        public string CustomerName { get => _customerName; set => _customerName = value; }
        public long SSN { get => _sSN; }
        public List<Account> Accounts { get => _accounts; set => _accounts = value; }

        public Customer(string name, long pNr)
        {
            _customerName = name;
            _sSN = pNr;
        }
        public override string ToString()
        {
            string customerInfo = $"{_customerName} | SSN: {_sSN}";
            return customerInfo;
        }
    }
}

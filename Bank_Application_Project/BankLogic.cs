using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application_Project
{
    public static class BankLogic //I made BankLogic and all of its methods static so that i wouldn't have to create an instance of the class in order to use the methods. I saw no point in that.
    {
        private static List<Customer> _customerList = new List<Customer>();
        public static List<Customer> CustomerList { get => _customerList; } //this property was a late addition, so that's why its own methods rarely use it.

        public static List<string> GetCustomers()
        {
            List<string> customerListOutput = new List<string>();
            foreach (Customer item in _customerList)
            {
                customerListOutput.Add(item.ToString());
            }
            return customerListOutput;
        }
        public static bool AddCustomer(string name, long pNr)
        {
            try
            {
                foreach (Customer item in _customerList)
                {
                    if (item.SSN == pNr ) { return false; }
                }

                Customer newCustomer = new Customer(name, pNr);
                _customerList.Add(newCustomer);
                return true;
            }
            catch
            {
                return false;
            }


        }
        public static List<string> GetCustomer(long pNr)
        {
            List<string> customerInfo = new List<string>();
            try
            {
                
                int find = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { find = i; }
                }
                if (find < 0)
                {
                    customerInfo.Add("No customer found");
                    return customerInfo;
                }
                else
                {
                    customerInfo.Add(_customerList[find].ToString());
                    foreach (Account item in _customerList[find].Accounts)
                        customerInfo.Add(item.ToString());
                    return customerInfo;
                }
            }
            catch
            {
                customerInfo.Clear();
                customerInfo.Add("Error encountered. Check your inputs");
                return customerInfo;
            }
        }
        public static string NewCustomerName(long pNr, string name)
        {
            try
            {
                int find = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { find = i; }
                }
                if (find < 0) { return "Failed"; }
                else
                {
                    _customerList[find].CustomerName = name;
                    return name;
                }
            }
            catch
            {
                return "Failed";
            }
        }
        public static int LogIn(long pNr)
        {
            int find = -1;
            try
            {
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { find = i; }
                }
                if (find >= 0)
                    return find;

                else
                    return -1;
            }
            catch
            {
                find = -2;
                return find;
            }
        }
        public static List<string> RemoveCustomer(long pNr)
        {
            List<string> customerRemoval = new List<string>();
            try
            {

                int find = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { find = i; }
                }
                if (find < 0)
                {
                    customerRemoval.Add("No customer found");
                    return customerRemoval;
                }
                else
                {
                    decimal totalBalance = 0;
                    decimal totalRent = 0;
                    customerRemoval.Add(_customerList[find].ToString());
                    foreach (Account item in _customerList[find].Accounts)
                    {
                        if (item.AccountType == "Savings Account")
                        {
                            customerRemoval.Add(item.ToString());
                            totalBalance += item.Balance;
                            totalRent += (item.InterestRate * item.Balance) - item.Balance;
                        }
                        else if (item.AccountType == "Credit Account")
                        {
                            CreditAccount cItem = item as CreditAccount;
                            customerRemoval.Add(cItem.ToString());
                            totalBalance += cItem.Balance;
                            decimal rentRate = cItem.InterestRate;
                            if (item.Balance < 0)
                                rentRate = cItem.DebtInterest;
                            totalRent += (rentRate * item.Balance) - item.Balance;
                        }
                    }
                    customerRemoval.Insert(0, "REMOVED:");
                    customerRemoval.Add($"Rent returned: ${totalRent}");
                    customerRemoval.Add($"Total money returned: ${totalBalance}");

                    _customerList.RemoveAt(find);    //this only removes all references to the customer and their accounts.
                    
                    return customerRemoval;
                }
            }
            catch
            {
                customerRemoval.Clear();
                customerRemoval.Add("Error encountered. Check your inputs");
                return customerRemoval;
            }
        }
        public static int AddSavingsAccount(long pNr)
        {
            try
            {
                int find = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { find = i; }
                }
                if (find < 0) { return -1; }

                SavingsAccount newAccount = new SavingsAccount();
                _customerList[find].Accounts.Add(newAccount);
                return newAccount.AccountNumber;
            }
            catch
            {
                return -1;
            }
        }
        public static int AddCreditAccount(long pNr)
        {
            try
            {
                int find = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { find = i; }
                }
                if (find < 0) { return -1; }

                CreditAccount newAccount = new CreditAccount();
                _customerList[find].Accounts.Add(newAccount);
                return newAccount.AccountNumber;
            }
            catch
            {
                return -1;
            }
        }
        public static string GetAccount(long pNr, int accountId)
        {
            try
            {
                int findCu = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { findCu = i; }
                }
                if (findCu >= 0)
                {
                    int findAc = -1;
                    for (int j = 0; j < _customerList[findCu].Accounts.Count(); j++)
                    {
                        if (_customerList[findCu].Accounts[j].AccountNumber == accountId) { findAc = j; }
                    }
                    if (findAc >= 0)
                    {
                        string accountInfo = $"SSN {pNr} | " + _customerList[findCu].Accounts[findAc].ToString();
                        return accountInfo;
                    }
                    else
                    {
                        return "Given account not found within given customer's profile";
                    }
                }
                else
                {
                    return "Customer with given SSN not found";
                }
            }
            catch
            {
                return "Error encountered. Check your inputs.";
            }
        }
        public static bool Deposit(long pNr, int accountId, decimal amount)
        {
            try
            {
                int findCu = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { findCu = i; }
                }
                if (findCu >= 0)
                {
                    int findAc = -1;
                    for (int j = 0; j < _customerList[findCu].Accounts.Count(); j++)
                    {
                        if (_customerList[findCu].Accounts[j].AccountNumber == accountId) { findAc = j; }
                    }
                    if (findAc >= 0)
                    {
                        _customerList[findCu].Accounts[findAc].Deposit(amount);
                        Transactions newDeposit = new Transactions(_customerList[findCu].Accounts[findAc].AccountNumber, "Deposited", amount, _customerList[findCu].Accounts[findAc].Balance);
                        _customerList[findCu].Accounts[findAc].TransactionList.Add(newDeposit);
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch
            {
                return false;
            }
        }
        public static bool Withdraw(long pNr, int accountId, decimal amount)
        {
            try
            {
                int findCu = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { findCu = i; }
                }
                if (findCu >= 0)
                {
                    int findAc = -1;
                    for (int j = 0; j < _customerList[findCu].Accounts.Count(); j++)
                    {
                        if (_customerList[findCu].Accounts[j].AccountNumber == accountId) { findAc = j; }
                    }
                    if (findAc >= 0)
                    {
                        bool withdrawResults = _customerList[findCu].Accounts[findAc].Withdraw(amount);
                        Transactions newWithdraw = new Transactions(_customerList[findCu].Accounts[findAc].AccountNumber, "Withdrawn", amount, _customerList[findCu].Accounts[findAc].Balance);
                        _customerList[findCu].Accounts[findAc].TransactionList.Add(newWithdraw);
                        return withdrawResults;
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch
            {
                return false;
            }
        }
        public static string CloseAccount(long pNr, int accountId)
        {
            try
            {
                int findCu = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { findCu = i; }
                }
                if (findCu >= 0)
                {
                    int findAc = -1;
                    for (int j = 0; j < _customerList[findCu].Accounts.Count(); j++)
                    {
                        if (_customerList[findCu].Accounts[j].AccountNumber == accountId) { findAc = j; }
                    }
                    if (findAc >= 0)
                    {
                        string accountInfo = $"REMOVED \nSSN: {pNr} | " + _customerList[findCu].Accounts[findAc].ToString();
                        if (_customerList[findCu].Accounts[findAc].AccountType == "Savings Account")
                        {
                            decimal rentReturn = _customerList[findCu].Accounts[findAc].InterestRate * _customerList[findCu].Accounts[findAc].Balance - _customerList[findCu].Accounts[findAc].Balance;
                            accountInfo += $"\nRent returned: ${rentReturn}";
                            accountInfo += $"\nTotal money returned: ${rentReturn + _customerList[findCu].Accounts[findAc].Balance}";
                        }
                        else if (_customerList[findCu].Accounts[findAc].AccountType == "Credit Account")
                        {
                            CreditAccount currentAccount = _customerList[findCu].Accounts[findAc] as CreditAccount; //i did this so that the following code could find the CreditAccount exclusive properties
                            decimal rentRate = currentAccount.InterestRate;
                            if (currentAccount.Balance < 0)
                                rentRate = currentAccount.DebtInterest;
                            decimal rentReturn = rentRate * currentAccount.Balance - currentAccount.Balance;
                            accountInfo += $"\nFinal rent: ${rentReturn}";
                            accountInfo += $"\nTotal final money taken/given: ${rentReturn + currentAccount.Balance}";
                        }
                        _customerList[findCu].Accounts.RemoveAt(findAc);
                        return accountInfo;
                    }
                    else
                    {
                        return "Given account not found within given customer's profile";
                    }
                }
                else
                {
                    return "Customer with given SSN not found";
                }
            }
            catch
            {
                return "Error encountered. Check your inputs.";
            }
        }
        public static List<string> GetTransactions(long pNr, int accountId)
        {
            List<string> transactionList = new List<string>();
            try
            {
                int findCu = -1;
                for (int i = 0; i < _customerList.Count(); i++)
                {
                    if (_customerList[i].SSN == pNr) { findCu = i; }
                }
                if (findCu >= 0)
                {
                    int findAc = -1;
                    for (int j = 0; j < _customerList[findCu].Accounts.Count(); j++)
                    {
                        if (_customerList[findCu].Accounts[j].AccountNumber == accountId) { findAc = j; }
                    }
                    if (findAc >= 0)
                    {
                        Account currentAc = _customerList[findCu].Accounts[findAc];
                        foreach (Transactions item in currentAc.TransactionList)
                        {
                            transactionList.Add(item.ToString());
                        }
                        return transactionList;
                    }
                    else
                    {
                        transactionList.Clear();
                        transactionList.Add("No account found within given customer's list");
                        return transactionList;
                    }
                }
                else
                {
                    transactionList.Clear();
                    transactionList.Add("No customer found with given SSN");
                    return transactionList;
                }
            }
            catch
            {
                transactionList.Clear();
                transactionList.Add("Error encountered. Check your inputs");
                return transactionList;
            }
        }
    }
}

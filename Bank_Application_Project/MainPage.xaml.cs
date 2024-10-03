using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Bank_Application_Project
{

    public sealed partial class MainPage : Page
    {
        int currentUser = -1;
        static readonly string nl = Environment.NewLine;
        public MainPage()
        {
            this.InitializeComponent();

            logInBlock.Visibility = Visibility.Visible;
            logInBlockText.Visibility = Visibility.Visible;  //hides all the customer specific functions behind a big block to reduce clutter and increase usability

            BankLogic.AddCustomer("Ultimate Bone", 19980617);
            BankLogic.AddCustomer("Sammi Mankini", 19941104);
            BankLogic.AddCustomer("Gruelsome Ben", 19540423);
            BankLogic.AddCustomer("Two-Boner Jimmy", 19780901);

            BankLogic.AddSavingsAccount(19980617);
            BankLogic.AddSavingsAccount(19941104);
            BankLogic.AddSavingsAccount(19941104);
            BankLogic.AddSavingsAccount(19540423);
            BankLogic.AddSavingsAccount(19780901);
            BankLogic.AddSavingsAccount(19980617);
            BankLogic.AddCreditAccount(19540423);
            BankLogic.AddCreditAccount(19980617);

            BankLogic.Deposit(19980617, 1001, 200);
            BankLogic.Deposit(19941104, 1002, 999);
            BankLogic.Deposit(19941104, 1003, 750);
            BankLogic.Deposit(19540423, 1004, 6.9M);
            BankLogic.Deposit(19780901, 1005, 33.33M);
            BankLogic.Deposit(19980617, 1006, 10000.5M);
            BankLogic.Withdraw(19540423, 1007, 300M);
            BankLogic.Deposit(19980617, 1008, 100M);
        }

        private void GetCustomersButton(object sender, RoutedEventArgs e)
        {
            outputText.Text = "";
            foreach (string customer in BankLogic.GetCustomers())
            {
                outputText.Text += customer + nl;
            }
        }

        private void LogInAction(object sender, RoutedEventArgs e)
        {
            logInResults.Text = "";
            try
            {
                int foundUser = BankLogic.LogIn(long.Parse(logInInput.Text));


                if (foundUser >= 0)
                {
                    currentUser = foundUser;
                    currentUserDiplay.Text = "Currently Logged In As" + nl + $"{BankLogic.CustomerList[foundUser].CustomerName}";
                    logInBlock.Visibility = Visibility.Collapsed;
                    logInBlockText.Visibility = Visibility.Collapsed;
                    logInResults.Text = $"Log In Successful";
                    logInInput.Text = "";
                }
                else if (foundUser == -1)
                {
                    logInResults.Text = $"Log In Failed, User not Found";
                }
                else
                {
                    logInResults.Text = $"Error Encountered, Retry";
                }
            }
            catch
            {
                logInResults.Text = $"Error Encountered, Retry";
            }
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = addUserInputName.Text;
                long pNr = long.Parse(addUserInputSSN.Text);
                bool result = BankLogic.AddCustomer(name, pNr);
                if (result)
                {
                    outputText.Text = "New User Created";
                    addUserInputName.Text = "";
                    addUserInputSSN.Text = "";
                }
                else
                {
                    outputText.Text = "Creation Failed" + nl + "User Might Already Exist";
                }
            }
            catch
            {
                outputText.Text = "Error Encountered, Recheck Inputs";
            }
        }

        private void DisplayUserInfo(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    List<string> userInfo = BankLogic.GetCustomer(BankLogic.CustomerList[currentUser].SSN);
                    foreach (string info in userInfo)
                    {
                        outputAccountText.Text += info + nl + nl;
                    }
                }
                catch
                {
                    outputAccountText.Text = "Error encountered";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature"; //i added these checks before adding the logInBlock so these are redundant now
            }
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            outputText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    List<string> removedUser = BankLogic.RemoveCustomer(BankLogic.CustomerList[currentUser].SSN);
                    foreach (string info in removedUser)
                    {
                        outputText.Text += info + nl + nl;
                    }

                    currentUser = -1;
                    logInBlock.Visibility = Visibility.Visible;
                    logInBlockText.Visibility = Visibility.Visible;
                }
                catch
                {
                    outputAccountText.Text = "Error encountered";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }

        private void AddSavingsAccount(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    int newAccount = BankLogic.AddSavingsAccount(BankLogic.CustomerList[currentUser].SSN);
                    if (newAccount > 0)
                    {
                        outputAccountText.Text = "Success" + nl + $"Account {newAccount} created for user {BankLogic.CustomerList[currentUser].CustomerName}";
                    }
                    else
                    {
                        outputAccountText.Text = "Account creation failed";
                    }
                }
                catch
                {
                    outputAccountText.Text = "Account creation failed";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            if (currentUser >= 0)
            {
                currentUser = -1;
                currentUserDiplay.Text = "Not Logged In";
                logInBlock.Visibility = Visibility.Visible;
                logInBlockText.Visibility = Visibility.Visible;
                outputText.Text = "Log Out Successful";
            }
            else
            {
                outputText.Text = "Please log in to use this feature ;)";  //it's a bit of a funny output, but it's true
            }
        }

        private void GetAccountInfo(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    string accountInfo = BankLogic.GetAccount(BankLogic.CustomerList[currentUser].SSN, int.Parse(accountNumberInput.Text));
                    outputAccountText.Text = accountInfo;
                }
                catch
                {
                    outputAccountText.Text = "Action Failed, please check inputs";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }

        private void RemoveAccount(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    string removedAccount = BankLogic.CloseAccount(BankLogic.CustomerList[currentUser].SSN, int.Parse(accountNumberInput.Text));
                    outputAccountText.Text = removedAccount;
                }
                catch
                {
                    outputAccountText.Text = "Action Failed, please check inputs";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }

        private void CashDeposit(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    bool success = BankLogic.Deposit(BankLogic.CustomerList[currentUser].SSN, int.Parse(accountNumberInput.Text), decimal.Parse(cashInput.Text));
                    if (success)
                    {
                        outputAccountText.Text = "Success!" + nl + $"${cashInput.Text} deposited into account {accountNumberInput.Text}";
                        cashInput.Text = "";
                    }
                    else
                    {
                        outputAccountText.Text = "Failed to deposit given amount";
                    }
                }
                catch
                {
                    outputAccountText.Text = "Error encountered. Please check your inputs";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }

        private void CashWithdraw(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    bool success = BankLogic.Withdraw(BankLogic.CustomerList[currentUser].SSN, int.Parse(accountNumberInput.Text), decimal.Parse(cashInput.Text));
                    if (success)
                    {
                        outputAccountText.Text = "Success!" + nl + $"${cashInput.Text} withdrawn from account {accountNumberInput.Text}";
                        cashInput.Text = "";
                    }
                    else
                    {
                        outputAccountText.Text = "Failed to withdraw given amount";
                    }
                }
                catch
                {
                    outputAccountText.Text = "Error encountered. Please check your inputs";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }

        private void AddCreditAccount(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    int newAccount = BankLogic.AddCreditAccount(BankLogic.CustomerList[currentUser].SSN);
                    if (newAccount > 0)
                    {
                        outputAccountText.Text = "Success" + nl + $"Account {newAccount} created for user {BankLogic.CustomerList[currentUser].CustomerName}";
                    }
                    else
                    {
                        outputAccountText.Text = "Account creation failed";
                    }
                }
                catch
                {
                    outputAccountText.Text = "Account creation failed";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }

        private void DisplayTransactions(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    List<string> accountInfo = BankLogic.GetTransactions(BankLogic.CustomerList[currentUser].SSN, int.Parse(accountNumberInput.Text));
                    outputAccountText.Text = BankLogic.CustomerList[currentUser].ToString() + nl + nl;
                    foreach (string item in accountInfo)
                    {
                        outputAccountText.Text += item + nl + nl;
                    }
                    
                }
                catch
                {
                    outputAccountText.Text = "Action Failed, please check inputs";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }

        private void NewName(object sender, RoutedEventArgs e)
        {
            outputAccountText.Text = "";
            if (currentUser >= 0)
            {
                try
                {
                    string newName = BankLogic.NewCustomerName(BankLogic.CustomerList[currentUser].SSN, newNameInput.Text);
                    outputAccountText.Text = $"Success" + nl + $"Changed name of customer with SSN {BankLogic.CustomerList[currentUser].SSN} to {newName}";
                    currentUserDiplay.Text = "Currently Logged In As" + nl + $"{newName}";
                    newNameInput.Text = "";
                }
                catch
                {
                    outputAccountText.Text = "Name change failed";
                }
            }
            else
            {
                outputAccountText.Text = "Please log in to use this feature";
            }
        }
    }
}

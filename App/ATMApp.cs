using ATM_App.Domain.Entities;
using ATM_App.Domain.Enums;
using ATM_App.Domain.Interfaces;
using ATM_App.UI;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM_App 
{   
    public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private const decimal minimumKeptAmount = 5;

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumberandPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuOption();
            }
        }

        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id = 1, AccountType = "Savings", FullName = "Dalayno Franklin", AccountNumber = 123456, CardNumber = 123456789, CardPin = 6052, AccountBalance = 50000.00m, IsLocked = false},
                new UserAccount{Id = 2, AccountType = "Savings", FullName = "Alana Reyes", AccountNumber = 123123, CardNumber = 987654321, CardPin = 0417, AccountBalance = 1000000.00m, IsLocked = false},
                new UserAccount{Id = 3, AccountType = "Checking", FullName = "Bailey Mac", AccountNumber = 654321, CardNumber = 0123456789, CardPin = 0527, AccountBalance = 200.00m, IsLocked = true},
                new UserAccount{Id = 4, AccountType = "Checking", FullName = "Yolanda Mac", AccountNumber = 0123, CardNumber = 012301230123, CardPin = 1126, AccountBalance = 1500.00m, IsLocked = false},
                new UserAccount{Id = 5, AccountType = "Checking", FullName = "Dalayno Franklin", AccountNumber = 410, CardNumber = 410410410, CardPin = 6052, AccountBalance = 500.00m, IsLocked = false},
                new UserAccount{Id = 6, AccountType = "Checking", FullName = "Dalayno Franklin", AccountNumber = 443, CardNumber = 443443443, CardPin = 6052, AccountBalance = 1500.00m, IsLocked = false},
            };

            _listOfTransactions = new List<Transaction>();
        }

        public void CheckUserCardNumberandPassword()
        {
            bool isCorrectLogin = false;
            
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach (UserAccount account in userAccountList)
                {
                    selectedAccount = account;

                    if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;

                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                        if (isCorrectLogin == false)
                        {
                            Utility.PrintMessage("\nInvalid card number or PIN", false);
                            selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                            if (selectedAccount.IsLocked)
                            {
                                AppScreen.PrintLockScreen();
                            }
                        }
                        Console.Clear();
                    }
                }
            }
        }

        private void ProcessMenuOption()
        {
            switch (Validator.Convert<int>("an option"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithdrawal();
                    break;
                case (int)AppMenu.SelfTransfer:
                    SelectAccount();
                    var selfTransfer = AppScreen.SelfTransferForm();
                    ProcessSelfTransfer(selfTransfer);
                    break;
                case (int)AppMenu.WireTransfer:
                    var wireTransfer = AppScreen.WireTransferForm();
                    ProcessWireTransfer(wireTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out. Please collect your ATM card.");
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid Option.", false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nMultiples of $1 or $5.\n");
            int transaction_amount = Validator.Convert<int>($"amount {AppScreen.cur}");

            Console.WriteLine("\nChecking and counting money.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            if (transaction_amount <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }

            if (transaction_amount % 1 != 0)
            {
                Utility.PrintMessage($"Enter deposit amount in multiples of $1 or $5.", false);
                return;
            }

            if (PreviewBankNotesCount(transaction_amount) == false)
            {
                Utility.PrintMessage($"You have cancelled your action.", false);
                return;
            }

            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amount, "");

            selectedAccount.AccountBalance += transaction_amount;

            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amount)} was successful.", true);

        }

        public void MakeWithdrawal()
        {
            int selectedAmount = AppScreen.SelectAmount();
            int transaction_amount;

            if (selectedAmount == -1)
            {
                MakeWithdrawal();
                return;
            }
            else if (selectedAmount != 0)
            {
                transaction_amount = selectedAmount;
            }
            else
            {
                transaction_amount = Validator.Convert<int>($"amount {AppScreen.cur}");
            }

            // input validation
            if (transaction_amount <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again", false);
                return;
            }

            if (transaction_amount % 1 != 0)
            {
                Utility.PrintMessage("You can only withdraw in multiples of $1 or $5. Please try again.", false);
                return;
            }

            // business logic
            if (transaction_amount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance does not have the funds to withdraw {Utility.FormatAmount(transaction_amount)}", false);
                return;
            }

            if (selectedAccount.AccountBalance - transaction_amount < minimumKeptAmount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have a minimum of {AppScreen.cur}" + minimumKeptAmount, false);
                return;
            }

            // Withdrawal to transaction objects
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, transaction_amount, "");

            // update account balance
            selectedAccount.AccountBalance -= transaction_amount;

            // success message
            Utility.PrintMessage($"You have successfully withdrawn {Utility.FormatAmount(transaction_amount)}.", true);

        }

        public static bool PreviewBankNotesCount(int amount)
        {
            int countFives = amount / 5;
            int countOnes = (amount % 5) / 1;
            Console.WriteLine("\nSummary");
            Console.WriteLine("------");
            Console.WriteLine($"{AppScreen.cur}5 X {countFives} = {5 * countFives}");
            Console.WriteLine($"{AppScreen.cur}1 X {countOnes} = {1 * countOnes}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            var transaction = new Transaction()
            {
                TransactionID = Utility.GetTransactionId(),
                UserBankAccountID = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc
            };

            _listOfTransactions.Add(transaction);
        }

        private void ProcessSelfTransfer(SelfTransfer selfTransfer)
        {
            if (selfTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again", true);
                return;
            }

            // check sender's account balance
            if (selfTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough funds to transfer {Utility.FormatAmount(selfTransfer.TransferAmount)}");
                return;
            }

            // check minimum amount
            if ((selectedAccount.AccountBalance - selfTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer failed. Your account need to have a minimum of {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            // check to see if recipient's account number is valid
            var selectedBankAccount = (from userAcc in userAccountList
                                                where userAcc.AccountNumber == selfTransfer.BankAccountNumber
                                                select userAcc).FirstOrDefault();

            if (selectedBankAccount == null)
            {
                Utility.PrintMessage("Transfer failed. Bank account number is invalid.", false);
                return;
            }

            // add transaction to transaction record -sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, selfTransfer.TransferAmount, $"Transfered to {selectedBankAccount.AccountNumber} ({selectedBankAccount.FullName})");

            // update sender's balance
            selectedAccount.AccountBalance -= selfTransfer.TransferAmount;

            // add transaction record -recipient
            InsertTransaction(selectedBankAccount.Id, TransactionType.Transfer, selfTransfer.TransferAmount, $"Transfered from {selectedAccount.AccountNumber}({selectedAccount.FullName})");

            // update receiver balance
            selectedBankAccount.AccountBalance += selfTransfer.TransferAmount;

            // print success message
            Utility.PrintMessage($"You have successfully transfered {Utility.FormatAmount(selfTransfer.TransferAmount)} to account {selfTransfer.BankAccountNumber}.", true);
        }

        private void ProcessWireTransfer(WireTransfer wireTransfer)
        {
            if (wireTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again", true);
                return;
            }

            // check sender's account balance
            if (wireTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough funds to transfer {Utility.FormatAmount(wireTransfer.TransferAmount)}");
                return;
            }

            // check minimum amount
            if ((selectedAccount.AccountBalance - wireTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer failed. Your account need to have a minimum of {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            // check to see if recipient's account number is valid
            var selectedBankAccountRecipient = (from userAcc in userAccountList
                                       where userAcc.AccountNumber == wireTransfer.RecipientBankAccountNumber
                                       select userAcc).FirstOrDefault();
            

            if (selectedBankAccountRecipient == null)
            {
                Utility.PrintMessage("Transfer failed. Recipient bank account number is invalid.", false);
                return;
            }

            // check recipient's name
            if (selectedBankAccountRecipient.FullName != wireTransfer.RecipientBankAccountName)
            {
                Utility.PrintMessage("Transfer failed. Recipient's name does not match.", false);
                return;
            }

            // add transaction to transaction record -sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, wireTransfer.TransferAmount, $"Transfered to {selectedBankAccountRecipient.AccountNumber} ({selectedBankAccountRecipient.FullName})");

            // update sender's balance
            selectedAccount.AccountBalance -= wireTransfer.TransferAmount;

            // add transaction record -recipient
            InsertTransaction(selectedBankAccountRecipient.Id, TransactionType.Transfer, wireTransfer.TransferAmount, $"Transfered from {selectedAccount.AccountNumber}({selectedAccount.FullName})");

            // update receiver balance
            selectedBankAccountRecipient.AccountBalance += wireTransfer.TransferAmount;

            // print success message
            Utility.PrintMessage($"You have successfully transfered {Utility.FormatAmount(wireTransfer.TransferAmount)} to {wireTransfer.RecipientBankAccountName}.", true);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _listOfTransactions.Where(t => t.UserBankAccountID == selectedAccount.Id).ToList();

            // check if there's a transaction
            if (filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transactions yet.", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Descriptions", "Amount " + AppScreen.cur);
                {
                    foreach (var transaction in filteredTransactionList)
                    {
                        table.AddRow(transaction.TransactionID, transaction.TransactionDate, transaction.TransactionType, transaction.Description, transaction.TransactionAmount);
                    }
                    table.Options.EnableCount = false;
                    table.Write();
                    Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
                }
            }
        }

        public void SelectAccount()
        {
            var table = new ConsoleTable("Id", "Bank Account Type", "Bank Account Number", "Bank Account Amount");
            {
                foreach (var account in userAccountList)
                {
                    if (selectedAccount.FullName == account.FullName && account.Id != selectedAccount.Id)
                    {
                        UserAccount otherAccount = account;
                        table.AddRow(otherAccount.Id, otherAccount.AccountType, otherAccount.AccountNumber, otherAccount.AccountBalance);
                    }
                }
                table.Options.EnableCount = false;
                table.Write();
            }
        }
    }
}

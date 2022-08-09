using ATM_App.Domain.Entities;
using System;

namespace ATM_App.UI
{
    public class AppScreen
    {
        internal const string cur = "$ ";

        internal static void Welcome()
        {
            //clears the console screen
            Console.Clear();

            //sets the title of the console window
            Console.Title = "My ATM App";

            //sets the foreground color to white
            Console.ForegroundColor = ConsoleColor.White;

            //set the welcome message 
            Console.WriteLine("\n\n--------------Welcome to My ATM App--------------\n\n");

            //prompts the user to input atm card
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("Note: Actual ATM machine will accept and validate a physical ATM card, read the card number and validate it");

            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new();

            tempUserAccount.CardNumber = Validator.Convert<long>("your card number");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your card PIN"));

            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking card number and PIN...");
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please go to the nearest branch to unlock your account. Thank you", true);
            Environment.Exit(1);
        }

        internal static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welcome back, {fullName}");
            Utility.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("-------My ATM App Menu-------");
            Console.WriteLine(":                           :");
            Console.WriteLine("1. Account Balance          :");
            Console.WriteLine("2. Deposit                  :");
            Console.WriteLine("3. Withdrawal               :");
            Console.WriteLine("4. Self Transfer            :");
            Console.WriteLine("5. Internal Transfer        :");
            Console.WriteLine("6. Transactions             :");
            Console.WriteLine("7. Logout                   :");
        }

        internal static void LogoutProgress()
        {
            Console.WriteLine("Thank you for using my ATM App");
            Utility.PrintDotAnimation();
            Console.Clear();
        }

        internal static int SelectAmount()
        {
            Console.WriteLine(" ");
            Console.WriteLine(":1.{0}200", cur);
            Console.WriteLine(":2.{0}100", cur);
            Console.WriteLine(":3.{0}50", cur);
            Console.WriteLine(":4.{0}20", cur);
            Console.WriteLine(":0.Other");

            int selectedAmount = Validator.Convert<int>("option:");

            switch (selectedAmount)
            {
                case 1:
                    return 200;
                case 2:
                    return 100;
                case 3:
                    return 50;
                case 4:
                    return 20;
                case 0:
                    return 0;
                default:
                    Utility.PrintMessage("Invalid input. Try again.", false);
                    return -1;
            }
        }

        internal static WireTransfer WireTransferForm()
        {
            WireTransfer wireTransfer = new();
            wireTransfer.RecipientBankAccountNumber = Validator.Convert<long>("recipient's account number:");
            wireTransfer.TransferAmount = Validator.Convert<decimal>($"amount {cur}");
            wireTransfer.RecipientBankAccountName = Utility.GetUserInput("recipient name: ");
            return wireTransfer;
        }

        internal static SelfTransfer SelfTransferForm()
        {
            SelfTransfer selfTransfer = new();
            selfTransfer.BankAccountNumber = Validator.Convert<long>("account number:");
            selfTransfer.TransferAmount = Validator.Convert<decimal>($"amount {cur}");
            return selfTransfer;
        }
    }
}

using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace ATM_App.UI
{
    public static class Utility
    {
        private static long tranId;
        private static readonly CultureInfo culture = new("en-US");

        public static void PressEnterToContinue()
        {
            Console.WriteLine("\n\nPress Enter to continue...\n");
            Console.ReadLine();
        }

        public static string GetUserInput(string prompt)
        {
            Console.WriteLine($"Enter {prompt}");
            return Console.ReadLine();
        }

        public static void PrintMessage(string msg, bool success = true)
        {
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(msg);
            Console.ResetColor();
            PressEnterToContinue();
        }

        public static string GetSecretInput(string prompt)
        {
            bool isPrompt = true;
            ConsoleKey key = new();
            var secretPassword = string.Empty;

            do
            {
                if (isPrompt)
                {
                    Console.WriteLine(prompt);
                }
                isPrompt = false;
                ConsoleKeyInfo inputKey = Console.ReadKey(intercept: true);
                key = inputKey.Key;

                if (inputKey.Key == ConsoleKey.Enter)
                {
                    if (secretPassword.Length == 4)
                    {
                        break;
                    }
                    else
                    {
                        PrintMessage("\nPlease enter 4 digits", false);
                        isPrompt = true;
                        secretPassword = secretPassword.Remove(0);
                        continue;
                    }
                }

                if (inputKey.Key == ConsoleKey.Backspace && secretPassword.Length > 0)
                {
                    Console.Write("\b \b");
                    secretPassword = secretPassword[0..^1];
                }
                else if (!char.IsControl(inputKey.KeyChar))
                {
                    Console.Write("*");
                    secretPassword += inputKey.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            return secretPassword.ToString();
        }



        public static void PrintDotAnimation(int timer=10)
        {
            for (int i = 0; i < timer; i++)
            {
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.Clear();
        }

        public static string FormatAmount(decimal amount)
        {
            return String.Format(culture, "{0:C2}", amount);
        }

        public static long GetTransactionId()
        {
            return ++tranId;
        }
    }
}

﻿using System;
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
            string asterisk = "";

            StringBuilder input = new();

            while (true)
            {
                if (isPrompt)
                {
                    Console.WriteLine(prompt);
                }
                    isPrompt = false;
                    ConsoleKeyInfo inputKey = Console.ReadKey(true);
                
                if (inputKey.Key == ConsoleKey.Enter)
                {
                    if (input.Length == 4)
                    {
                        break;
                    }
                    else
                    {
                        PrintMessage("\nPlease enter 4 digits", false);
                        isPrompt = true;
                        input.Clear();
                        continue;
                    }
                }
                
                if (inputKey.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                }
                else if (inputKey.Key != ConsoleKey.Backspace)
                {
                    input.Append(inputKey.KeyChar);
                    Console.Write(asterisk + "*");
                }
            }
            return input.ToString();
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

﻿using System;

namespace yoc_csharp_banking
{
    public class UpdatePassword
    {
        public const int ATTEMPTS = 3;

        public static bool Update(string id)
        {
            if (!VerifyIdentity(id))
                return false;
            if (!YesNoInput("To confirm, would you like to change your password (Y/N): "))
                return false;
            Console.Clear();
            string newPassword = GetNewPasswordInput();
            SetPassword(id, newPassword);
            return CheckSuccess(id, newPassword);
        }

        public static bool YesNoInput(string prompt)
        {
            Console.Write(prompt);
            char answer = char.ToUpper(Console.ReadLine()[0]);
            return answer == 'Y';
        }

        public static string GetPasswordInput()
        {
            var password = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();

            return password;
        }
        private static bool VerifyIdentity(string id)
        {
            Console.WriteLine("For security purposes, we need to verify your indentity.");

            for (int i = 0; i < ATTEMPTS; i++)
            {
                if (i != 0)
                {
                    if (!YesNoInput("Try again? (Y/N): "))
                    {
                        return false;
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }

                Console.WriteLine("Please enter your current password below.");


                string password = GetPasswordInput();
                string passwordDigest = encryptSHA256.encrypt(password);

                string correctPasswordDigest = main.Read(null, null,
                    $"SELECT password FROM bankData WHERE id = '{id}'")[0];

                if (passwordDigest == correctPasswordDigest)
                {
                    Console.Clear();
                    Console.WriteLine("We have verified your identity.\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Incorrect password!\n");
                }
            }
            Console.WriteLine("Sorry. \nYou have incorrectly guessed your password too many times.");
            return false;
        }

        private static string GetNewPasswordInput()
        {
            Console.WriteLine("Please enter your new password below:");
            return GetPasswordInput();
        }


        private static void SetPassword(string id, string newPassword)
        {
            string digest = encryptSHA256.encrypt(newPassword);
            main.CreateUpdateOrDelete($"UPDATE bankData SET password = '{digest}' WHERE id = '{id}'");
        }

        private static bool CheckSuccess(string id, string newPassword)
        {
            string newPasswordDigest = encryptSHA256.encrypt(newPassword);
            string storedPasswordDigest = main.Read(null, null,
                $"SELECT password FROM bankData WHERE id = '{id}'")[0];
            if (newPasswordDigest == storedPasswordDigest)
            {
                Console.WriteLine("Your password was sucessfully changed.");
                Console.WriteLine("Press ENTER to continue:");
                Console.ReadLine();
                return true;
            }
            Console.WriteLine("Unforunately, we have encountered an error. \nSorry, your password was not changed.");
            Console.WriteLine();
            return false;
        }
    }
}

﻿using System;

namespace yoc_csharp_banking
{
    class userManagement
    {
        public static void addUser(string name = "default", string password = "default", int balance = 0)
        {
            Console.Clear();
            if (name == "default")
            {
                Console.WriteLine($"Please enter user's name: ");
                name = Console.ReadLine();
                Console.WriteLine($"Please enter {name}'s password: ");
                password = Console.ReadLine();
            }

            Console.WriteLine($"Please enter {name}'s balance: ");
            bool worked = int.TryParse(Console.ReadLine(), out balance);
            if (!(worked))
            {
                Console.WriteLine("That is not a valid balance.");
                addUser(name, password);
            }

            main.CreateUpdateOrDelete($"INSERT INTO testTable (name, password, balance) VALUES ('{name}', '{encryptSHA256.encrypt(password)}', {balance})");
        }

        public static void removeUser()
        {
            Console.WriteLine("Remove Data Here! ");
            //main.CreateUpdateOrDelete($"DELETE STUFF");
        }
    }
}

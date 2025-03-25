using System;
using System.Collections.Generic;

namespace HabitTracker
{
    class User
    {
        private Database database;

        public User(string dbName)
        {
            this.database = new Database(dbName);
        }

        public void DbSession()
        {
            Console.WriteLine("Welcome to the habit tracker!");
            string menuStr = "Menu options:\n" +
                               "C: create a new habit to log\n" +
                               "I: insert new record for a given habit" +
                               "U: update a record for a given habit" +
                               "D: delete a given habit record" +
                               "V: view records for a given habit" +
                               "E: exit";
            Console.WriteLine(menuStr);
            var done = false;
            while (!done)
            {
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "C":
                        break;
                    case "I":
                        break;
                    case "U":
                        break;
                    case "D":
                        break;
                    case "V":
                        break;
                    case "E":
                        done = true;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Invalid menu choice: try again\n");
                        break;
                }
            }
        }
    }
}

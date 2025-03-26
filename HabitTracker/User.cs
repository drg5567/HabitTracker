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
                               "I: insert new record for a given habit\n" +
                               "U: update a record for a given habit\n" +
                               "D: delete a given habit record or table\n" +
                               "V: view records for a given habit\n" +
                               "E: exit\n";
            Console.WriteLine(menuStr);
            var done = false;
            while (!done)
            {
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "C":
                        CreateTable();
                        break;
                    case "I":
                        InsertRecord();
                        break;
                    case "U":
                        UpdateRecord();
                        break;
                    case "D":
                        DeleteRecord();
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

        private void CreateTable()
        {
            Console.WriteLine("Enter new habit name:");
            var newHabit = Console.ReadLine();
            if (newHabit == "" || newHabit == null)
            {
                Console.WriteLine("Habit name empty, returning to main menu...");
            }
            else
            {
                this.database.CreateTable(newHabit);
                Console.WriteLine($"{newHabit} table created");
            }
        }

        private void InsertRecord()
        {
            // TODO: add check to prevent multiple records for the same date
            Console.WriteLine("Enter table name:");
            var habitName = Console.ReadLine();
            if (habitName == "" || habitName == null)
            {
                Console.WriteLine("Habit name empty, returning to main menu...");
            }
            else
            {
                var recDate = InputDate();
                var recNum = InputNumber();
                this.database.InsertRecord(habitName, recDate, recNum);
                Console.WriteLine("Record Inserted, returning to menu...");
            }
        }

        private void UpdateRecord()
        {
            Console.WriteLine("Enter table name:");
            var habitName = Console.ReadLine();
            if (habitName == "" || habitName == null)
            {
                Console.WriteLine("Habit name empty, returning to main menu...");
            }
            else
            {
                var recDate = InputDate();
                var recNum = InputNumber();
                this.database.UpdateRecord(habitName, recDate, recNum);
                Console.WriteLine("Record Updated, returning to menu...");
            }
        }

        private void DeleteRecord()
        {
            Console.WriteLine("Enter table name:");
            var habitName = Console.ReadLine();
            if (habitName == "" || habitName == null)
            {
                Console.WriteLine("Habit name empty, returning to main menu...");
            }
            else
            {
                var recDate = InputDate();
                this.database.DeleteRecord(habitName, recDate);
                Console.WriteLine("Record Deleted, returning to menu...");
            }
        }

        private string InputDate()
        {
            Console.WriteLine("Enter date of record (must be in format MM-DD-YYYY");
            while (true)
            {
                var dateStr = Console.ReadLine();
                if (dateStr != null && dateStr.Length == 10)
                {
                    var month = Convert.ToInt32(dateStr.Substring(0, 2));
                    var day = Convert.ToInt32(dateStr.Substring(3, 5));
                    var year = Convert.ToInt32(dateStr.Substring(6, 10));

                    if (1 <= month && month <= 12 &&
                        1 <= day && day <= 31 &&
                        1 <= year && year <= 9999)
                    {
                        if (dateStr[2] == '-' && dateStr[5] == '-')
                        {
                            return dateStr;
                        }
                    }
                }
                Console.WriteLine("Invalid date entry, try again");
            }
        }

        private int InputNumber()
        {
            Console.WriteLine("Enter number of times habit was completed (must be >= 1):");
            int result;
            while (true)
            {
                var numStr = Console.ReadLine();
                if (Int32.TryParse(numStr, out result))
                {
                    if (result > 0)
                    {
                        return result;
                    }
                }
                else
                {
                    Console.WriteLine("Not a valid input, try again...");
                }
            }
        }
    }
}

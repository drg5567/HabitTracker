using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter database name:");
            var dbName = Console.ReadLine();
            if (dbName == null )
            {
                dbName = "habit.db";
            }
            User user = new User(dbName);
            user.DbSession();
        }
    }
}
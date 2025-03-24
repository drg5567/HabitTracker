using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    class Database
    {
        private string dbName;

        public Database(string dbName)
        {
            this.dbName = dbName;
            createDBIfNonExistant();
        }

        public void createDBIfNonExistant()
        {
            // Create two basic tables to populate the database
            CreateTable("exercise");
            CreateTable("hydration");

            // TODO: add random values to the tables

        }

        public void CreateTable(string tableName)
        {
            using (var connection =new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE $name (" +
                        "id INT PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "date TEXT NOT NULL," +
                        "numTimes INT NOT NULL);";
                command.Parameters.AddWithValue("$name", tableName);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Dispose();
                }
            }
        }
    }
}

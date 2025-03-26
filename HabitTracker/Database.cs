using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ConsoleTableExt;

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
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"CREATE TABLE {tableName} (" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                        "date TEXT NOT NULL," +
                        "numTimes INTEGER NOT NULL);";

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

        public void InsertRecord(string tableName, string date, int occurrence)
        {
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO {tableName} (date, numTimes) VALUES (@date, @val);";
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@val", occurrence);

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

        public void UpdateRecord(string tableName, string date, int occurrence)
        {
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"UPDATE {tableName} SET numTimes = @val WHERE date = @date;";
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@val", occurrence);

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

        public void DeleteRecord(string tableName, string date)
        {
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM {tableName} WHERE date = @date;";
                command.Parameters.AddWithValue("@date", date);

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

        public void SearchTable(string tableName, int rowId = -1, string fromDate = "",
            string toDate = "", int minOccur = 0, int maxOccur = 0)
        {
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                var cmdTxt = $"SELECT * FROM {tableName} ";
                var changed = false;
                if (rowId >= 0)
                {
                    cmdTxt += "WHERE id = @id";
                    changed = true;
                }
                if (fromDate != "")
                {
                    if (!changed)
                    {
                        cmdTxt += "WHERE ";
                    }
                    else
                    {
                        cmdTxt += "AND ";
                    }
                    cmdTxt += "date <= @from ";
                }
                if (toDate != "")
                {
                    if (!changed)
                    {
                        cmdTxt += "WHERE ";
                    }
                    else
                    {
                        cmdTxt += "AND ";
                    }
                    cmdTxt += "date > @to ";
                }
                if (minOccur > 0)
                {
                    if (!changed)
                    {
                        cmdTxt += "WHERE ";
                    }
                    else
                    {
                        cmdTxt += "AND ";
                    }
                    cmdTxt += "numTimes >= @min";
                }
                if (maxOccur > 0)
                {
                    if (!changed)
                    {
                        cmdTxt += "WHERE ";
                    }
                    else
                    {
                        cmdTxt += "AND ";
                    }
                    cmdTxt += "numTimes < @max";
                }

                command.CommandText = cmdTxt;
                command.Parameters.AddWithValue("@id", rowId);
                command.Parameters.AddWithValue("@from", fromDate);
                command.Parameters.AddWithValue("@to", toDate);
                command.Parameters.AddWithValue("@min", minOccur);
                command.Parameters.AddWithValue("@max", maxOccur);

                var tableData = new List<List<object>>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var date = reader.GetString(1);
                        var occurrence = reader.GetString(2);
                        tableData.Add(new List<object> {date, occurrence});
                    }
                    ConsoleTableBuilder
                            .From(tableData)
                            .WithFormat(ConsoleTableBuilderFormat.Alternative)
                            .ExportAndWriteLine(TableAligntment.Left);
                }
            }
        }
    }
}

﻿using Microsoft.Data.Sqlite;
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

        private void createDBIfNonExistant()
        {
            // Create two basic tables to populate the database, 'exercise' and 'hydration'
            var tablesExist = false;
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name IN ('exercise', 'hydration');";
                try
                {
                    var result = command.ExecuteReader();
                    if (result.HasRows)
                    {
                        tablesExist = true;
                    }
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
            if (!tablesExist)
            {
                CreateTable("exercise");
                CreateTable("hydration");

                PopulateDB("exercise");
                PopulateDB("hydration");
            }
        }

        private void PopulateDB(string tableName)
        {
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();
                var command = connection.CreateCommand();

                Random r = new Random();
                List<string> dateList = new List<string>();
                while (dateList.Count < 100)
                {
                    DateTime lastYear = DateTime.Today.AddYears(-1);
                    int randNum = r.Next(0, 365);
                    DateTime randDay = lastYear.AddDays(randNum);
                    string date = randDay.ToString("yyyy-MM-dd");
                    if (dateList.Contains(date))
                    {
                        continue;
                    }
                    dateList.Add(date);

                    int occurrence = r.Next(1, 5);

                    command.CommandText = $"INSERT INTO {tableName} (date, numTimes) VALUES (@date, @val);";
                    command.Parameters.Clear();
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
                }
                command.Dispose();
                connection.Dispose();
            }
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

                // Switch date format for storage
                string formattedDate = ConvertDate(date);
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

                // Switch date format for storage
                string formattedDate = ConvertDate(date);
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

                // Switch date format for storage
                string formattedDate = ConvertDate(date);
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

        public void DeleteTable(string tableName)
        {
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"DROP TABLE IF EXISTS {tableName};";

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

        public void SearchTable(string tableName, string fromDate, string toDate = "")
        {
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Switch date format for storage
                string formattedFromDate = ConvertDate(fromDate);
                var cmdTxt = $"SELECT * FROM {tableName} WHERE date ";
                if (toDate == "")
                {
                    command.CommandText = cmdTxt + "= @from;";
                    command.Parameters.AddWithValue("@from", formattedFromDate);
                }
                else
                {
                    // Switch date format for storage
                    string formattedToDate = ConvertDate(toDate);

                    command.CommandText = cmdTxt + "BETWEEN @from AND @to ORDER BY date DESC;"; ;
                    command.Parameters.AddWithValue("@from", formattedFromDate);
                    command.Parameters.AddWithValue("@to", formattedToDate);
                }

                var tableData = new List<List<object>>();
                tableData.Add(new List<object> { "date:", "occurrence:" });
                try
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var date = reader.GetString(1);
                        var occurrence = reader.GetString(2);
                        tableData.Add(new List<object> { date, occurrence });
                    }
                    ConsoleTableBuilder
                            .From(tableData)
                            .WithFormat(ConsoleTableBuilderFormat.Alternative)
                            .ExportAndWriteLine(TableAligntment.Left);
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

        public void ListTables()
        {
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";

                var tableData = new List<List<object>>();
                try
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var date = reader.GetString(0);
                        tableData.Add(new List<object> { date });
                    }
                    ConsoleTableBuilder
                            .From(tableData)
                            .WithFormat(ConsoleTableBuilderFormat.Alternative)
                            .ExportAndWriteLine(TableAligntment.Left);
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

        public bool IsDateAvailable(string tableName, string date)
        {
            var dateAvailable = false;
            using (var connection = new SqliteConnection("Data Source=" + this.dbName))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Switch date format for storage
                string formattedDate = ConvertDate(date);
                command.CommandText = $"SELECT * FROM {tableName} WHERE date = @date;";
                try
                {
                    var result = command.ExecuteReader();
                    if (!result.HasRows)
                    {
                        dateAvailable = true;
                    }
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
            return dateAvailable;
        }

        private string ConvertDate(string dateStr)
        {
            DateTime parsedDate = DateTime.ParseExact(dateStr, "MM-dd-yyyy", null);
            return parsedDate.ToString("yyyy-MM-dd");
        }
    }
}

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
        private string db_name;

        public Database(string db_name)
        {
            this.db_name = db_name;
            createDBIfNonExistant();
        }

        public void createDBIfNonExistant()
        {
            using (var connection = new SqliteConnection("Data Source=" + this.db_name))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "";

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

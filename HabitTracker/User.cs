namespace HabitTracker
{
    /// <summary>
    /// Represents a user connection to a given habit tracker database. Contains all user functionality
    /// methods.
    /// </summary>
    class User
    {
        private readonly Database database;

        public User(string dbName)
        {
            this.database = new Database(dbName);
        }

        /// <summary>
        /// The main connection loop for the user. Opens the main menu and allows the user to
        /// perform actions to insert, view, and modify habit records.
        /// </summary>
        public void DbSession()
        {
            Console.WriteLine("Welcome to the habit tracker!");
            string menuStr = "Menu options:\n" +
                               "C: create a new habit to log\n" +
                               "I: insert new record for a given habit\n" +
                               "U: update a record for a given habit\n" +
                               "D: delete menu\n" +
                               "V: view menu\n" +
                               "H: display main menu options\n" +
                               "W: clear screen\n" +
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
                        DeleteRecords();
                        break;
                    case "V":
                        ViewRecords();
                        break;
                    case "H":
                        Console.WriteLine(menuStr);
                        break;
                    case "E":
                        done = true;
                        Console.Clear();
                        break;
                    case "W":
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Invalid menu choice: try again\n");
                        break;
                }
            }
        }

        /// <summary>
        /// Prompts the user to enter a table name and initiates a create table action to the database.
        /// </summary>
        private void CreateTable()
        {
            var newHabit = InputTableName();
            this.database.CreateTable(newHabit);
            Console.WriteLine($"{newHabit} table created");
        }

        /// <summary>
        /// Prompts the user to enter information regarding inserting a record for a given habit.
        /// Attempts to enter record into the database.
        /// </summary>
        private void InsertRecord()
        {
            var habitName = InputTableName();
            var recDate = InputDate();
            var recNum = InputNumber();
            var dateAvailable = this.database.IsDateAvailable(habitName, recDate);
            if (dateAvailable)
            {
                this.database.InsertRecord(habitName, recDate, recNum);
                Console.WriteLine("Record Inserted, returning to main menu...");
            }
            else
            {
                Console.WriteLine("Operation canceled, date is already taken for given table");
                Console.WriteLine("Returning to main menu...");
            }
        }

        /// <summary>
        /// Prompts the user to enter information regarding updating a record for a given habit.
        /// Attempts to enter new record data into the database.
        /// </summary>
        private void UpdateRecord()
        {
            var habitName = InputTableName();
            var recDate = InputDate();
            Console.WriteLine("New value number:\n");
            var recNum = InputNumber();
            this.database.UpdateRecord(habitName, recDate, recNum);
            Console.WriteLine("Record Updated, returning to menu...");
        }

        /// <summary>
        /// Main user functionality to delete records. Displays a menu that allows users to delete
        /// single record values or entire tables.
        /// </summary>
        private void DeleteRecords()
        {
            var menuStr = "Delete Menu:\n" +
                "R: delete a single record for a given table and date\n" +
                "T: delete an entire table\n" +
                "H: display menu options\n" +
                "W: clear screen\n" +
                "E: return to main menu\n";
            Console.WriteLine(menuStr);
            while (true)
            {
                var selection = Console.ReadLine();
                if (selection == "R")
                {
                    var habitName = InputTableName();
                    var recDate = InputDate();
                    this.database.DeleteRecord(habitName, recDate);
                    Console.WriteLine("Record Deleted");
                }
                else if (selection == "T")
                {
                    var habitName = InputTableName();
                    this.database.DeleteTable(habitName);
                }
                else if (selection == "H")
                {
                    Console.WriteLine(menuStr);
                }
                else if (selection == "W")
                {
                    Console.Clear();
                }
                else if (selection == "E")
                {
                    Console.WriteLine("Exiting delete menu...");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input, try again");
                }
            }
        }

        /// <summary>
        /// Main user functionality to delete records. Displays a menu that allows users to view
        /// all active tables, single record values, or records within date ranges.
        /// </summary>
        private void ViewRecords()
        {
            var menuStr = "View Menu:\n" +
                "T: view table list\n" +
                "S: view a single record for a given table and date\n" +
                "M: view multiple records for a given date range\n" +
                "H: display menu options\n" +
                "W: clear screen\n" +
                "E: return to main menu\n";
            Console.WriteLine(menuStr);
            while (true)
            {
                var selection = Console.ReadLine();
                if (selection == "T")
                {
                    this.database.ListTables();
                }
                else if (selection == "S")
                {
                    var habitName = InputTableName();
                    var dateStr = InputDate();
                    this.database.SearchTable(habitName, dateStr);
                }
                else if (selection == "M")
                {
                    var habitName = InputTableName();
                    Console.WriteLine("From date:");
                    var fromDate = InputDate();
                    Console.WriteLine("To date:");
                    var toDate = InputDate();
                    this.database.SearchTable(habitName, fromDate, toDate);
                }
                else if (selection == "H")
                {
                    Console.WriteLine(menuStr);
                }
                else if (selection == "W")
                {
                    Console.Clear();
                }
                else if (selection == "E")
                {
                    Console.WriteLine("Exiting view menu...");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input, try again");
                }
            }
        }

        /// <summary>
        /// Prompts the user to enter a valid date value
        /// </summary>
        /// <returns></returns>
        private string InputDate()
        {
            Console.WriteLine("Enter date of record (must be in format MM-DD-YYYY)\n" +
                "Enter T to use today's date");
            while (true)
            {
                var dateStr = Console.ReadLine();
                if (dateStr != null && dateStr.Length == 10)
                {
                    var month = Convert.ToInt32(dateStr.Substring(0, 2));
                    var day = Convert.ToInt32(dateStr.Substring(3, 2));
                    var year = Convert.ToInt32(dateStr.Substring(6));

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
                else if (dateStr == "T")
                {
                    dateStr = DateTime.Now.ToString("MM-dd-yyyy");
                    return dateStr;
                }
                Console.WriteLine("Invalid date entry, try again");
            }
        }

        /// <summary>
        /// Prompts the user to enter a valid number value
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Prompts the user to enter a valid table name value
        /// </summary>
        /// <returns></returns>
        private string InputTableName()
        {
            Console.WriteLine("Enter table name:");
            while (true)
            {
                var habitName = Console.ReadLine();
                if (habitName == "" || habitName == null)
                {
                    Console.WriteLine("Habit name empty, try again");
                }
                else
                {
                    return habitName;
                }
            }
        }
    }
}

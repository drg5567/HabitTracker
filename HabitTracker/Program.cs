namespace HabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter database name (must end with '.db'):");
            var dbName = "habit.db";
            var valid = false;
            while (!valid)
            {
                var name = Console.ReadLine();
                if (name == "" || name == null)
                {
                    Console.WriteLine($"Resulting to default name '{dbName}'");
                    valid = true;
                }
                else if (name.Length < 4 || name.Substring(name.Length - 3) != ".db")
                {
                    Console.WriteLine("Invalid database name");
                }
                else
                {
                    dbName = name;
                    valid = true;
                }
            }
            User user = new User(dbName);
            user.DbSession();
        }
    }
}
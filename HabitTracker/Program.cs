namespace HabitTracker;

class Program
{
    static void Main(string[] args)
    {
        Database database = new Database();
        AbstractMenu mainMenu = new MainMenu();

        Console.ReadLine();
        database.Dispose();
    }
}

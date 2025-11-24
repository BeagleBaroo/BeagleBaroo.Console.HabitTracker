namespace HabitTracker;

class Program
{
    static void Main(string[] args)
    {
        Database database = new Database();
        AbstractMenu mainMenu = new MainMenu(database);
        mainMenu.Run();
        database.Dispose();
    }
}

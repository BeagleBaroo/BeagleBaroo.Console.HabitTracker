namespace HabitTracker.Menus;

public class MainMenu : AbstractMenu
{
    private UpsertHabitMenu _newHabitMenu;
    private ExistingHabitMenu _editHabitMenu;
    public MainMenu(Database database)
    {
        ValidOptions.Add("a");
        ValidOptions.Add("v");
        ValidOptions.Add("x");

        _newHabitMenu = new UpsertHabitMenu(database);
        _editHabitMenu = new ExistingHabitMenu(database, _newHabitMenu);
    }

    public override void Run(Habit? habit = null)
    {
        GetMenuText();
        PrintMenuText();
        string selectedMenuOption = GetUserInput(ValidOptions);
        GetNextMenu(selectedMenuOption);
    }

    protected override void GetMenuText(string option = "")
    {
        Console.Clear();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"Welcome to HabitTracker!\n");
        stringBuilder.Append($"Type \"a\" followed by enter to add a new habit log\n");
        stringBuilder.Append($"Type \"v\" followed by enter to view and edit current habit logs\n");
        stringBuilder.Append($"Type \"x\" followed by enter to exit HabitTracker\n");
        MenuText = stringBuilder.ToString();
    }

    protected override void GetNextMenu(string selectedMenuOption)
    {
        switch (selectedMenuOption)
        {
            case "a":
                _newHabitMenu.Run();
                Run();
                break;
            case "v":
                _editHabitMenu.Run();
                Run();
                break;
            case "x":
                break;
            default:
                Run();
                break;
        }
    }
}


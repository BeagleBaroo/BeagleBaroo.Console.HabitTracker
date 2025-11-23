namespace HabitTracker.Menus;

public class MainMenu : AbstractMenu
{
    public MainMenu()
    {
        ValidMenuOptions.Add("a");
        ValidMenuOptions.Add("v");
        ValidMenuOptions.Add("x");
    }

    public override void Run()
    {
        GetMenuText();
        PrintMenuText();
        string selectedMenuOption = GetSelectedMenuOption();
        GetNextMenu(selectedMenuOption);
    }

    protected override void GetMenuText()
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
                AbstractMenu newHabitMenu = new NewHabitMenu();
                break;
            case "v":
                AbstractMenu editHabitMenu = new EditHabitMenu();
                break;
            case "x":
                break;
            default:
                GetMenuText();
                break;
        }
    }
}


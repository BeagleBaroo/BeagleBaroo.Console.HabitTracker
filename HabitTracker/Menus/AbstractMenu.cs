namespace HabitTracker.Menus;

public abstract class AbstractMenu
{
    public AbstractMenu()
    {
        ValidMenuOptions = new List<string>();
    }
    protected string? MenuText { get; set; }
    protected List<string> ValidMenuOptions;
    protected void PrintMenuText()
    {
        Console.WriteLine(MenuText ?? "");
    }

    protected string GetSelectedMenuOption()
    {
        string selectedMenuOption = Console.ReadLine() ?? "";
        while (ValidMenuOptions.Contains(selectedMenuOption) is false)
        {
            Console.WriteLine("\nOops! That is not a valid option, please try again:\n");
            selectedMenuOption = Console.ReadLine() ?? "";
        }

        return selectedMenuOption;

    }

    public abstract void Run();
    protected abstract void GetMenuText();
    protected abstract void GetNextMenu(string selectedMenuOption);
}


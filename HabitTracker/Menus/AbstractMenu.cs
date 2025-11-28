namespace HabitTracker.Menus;

public abstract class AbstractMenu
{
    public AbstractMenu()
    {
        ValidOptions = new List<string>();
    }
    protected string? MenuText { get; set; }
    protected List<string> ValidOptions;
    protected void PrintMenuText()
    {
        Console.WriteLine(MenuText ?? "");
    }

    protected string GetUserInput(List<string>? validOptions = null)
    {
        string userInput = Console.ReadLine() ?? "";
        if (validOptions is not null && validOptions.Count > 0)
        {
            while (validOptions.Contains(userInput) is false)
            {
                Console.WriteLine("\nOops! That is not a valid option, please try again:\n");
                userInput = (Console.ReadLine() ?? "").ToLowerInvariant();
            }
        }
        return userInput;
    }

    public abstract void Run(Habit? habit = null);
    protected abstract void GetMenuText(string option = "");
    protected abstract void GetNextMenu(string selectedMenuOption, string action = "");
}


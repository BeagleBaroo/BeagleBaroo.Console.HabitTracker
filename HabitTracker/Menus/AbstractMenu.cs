namespace HabitTracker.Menus;

public abstract class AbstractMenu : IMenu
{
    protected string? MenuText { get; set; }
    protected void PrintMenuText()
    {
        Console.WriteLine(MenuText ?? "");
    }

    public abstract void GenerateMenuText();
}


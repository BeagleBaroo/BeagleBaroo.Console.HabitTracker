
namespace HabitTracker.Menus;

public class ExistingHabitMenu : AbstractMenu
{
    private UpsertHabitMenu _newHabitMenu;
    private List<Habit> _existingHabits;
    private Database _database;
    public ExistingHabitMenu(Database database, UpsertHabitMenu newHabitMenu)
    {
        _newHabitMenu = newHabitMenu;
        _database = database;
        _existingHabits = new List<Habit>();
    }

    public override void Run(Habit? habit = null)
    {
        _existingHabits = _database.Read();
        GetMenuText();
        PrintMenuText();

        ValidOptions.Add("x");
        string selectedMenuOption = GetUserInput();
        GetNextMenu(selectedMenuOption);
    }

    protected override void GetMenuText(string option = "")
    {
        Console.Clear();
        StringBuilder stringBuilder = new StringBuilder();

        if (_existingHabits.Count == 0)
        {
            stringBuilder.Append($"There are no habits to view/edit, pleas press \"x\" folowed by enter to return to the main menu:\n");
        }
        else
        {
            stringBuilder.Append($"Please type the nuber of the of the habit you wish to edit, followed by enter or press \"x\" folowed by enter to return to the main menu:\n");
            for (int i = 0; i < _existingHabits.Count; i++)
            {
                ValidOptions.Add((i + 1).ToString());
                stringBuilder.Append($"{i + 1}. Drink {_existingHabits[i].Quantity} cups of {_existingHabits[i].Description} on {_existingHabits[i].Date}\n");
            }
        }
        MenuText = stringBuilder.ToString();
    }

    protected override void GetNextMenu(string selectedMenuOption)
    {
        switch (true)
        {
            case bool _ when int.TryParse(selectedMenuOption, out int parseInt):
                Habit toUpdate = _existingHabits[parseInt - 1];
                _newHabitMenu.Run(toUpdate);
                break;
            case bool _ when selectedMenuOption.Equals("x"):
                break;
            default:
                Run();
                break;
        }
    }
}
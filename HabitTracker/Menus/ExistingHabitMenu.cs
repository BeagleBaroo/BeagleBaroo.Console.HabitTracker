
using System.Runtime.CompilerServices;

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
        string selectedMenuOption = GetUserInput(ValidOptions);

        ValidOptions.RemoveAll(vo => vo is not "x");
        GetActionText();
        PrintMenuText();
        string action = GetUserInput(ValidOptions);

        GetNextMenu(selectedMenuOption, action);
    }

    protected override void GetMenuText(string option = "")
    {
        Console.Clear();
        StringBuilder stringBuilder = new StringBuilder();

        if (_existingHabits.Count == 0)
        {
            stringBuilder.Append($"There are no habits to view/edit/delete, please press \"x\" folowed by enter to return to the main menu:\n");
        }
        else
        {
            stringBuilder.Append($"Please type the nuber of the of the habit you wish to edit/delete followed by enter or press\n");
            stringBuilder.Append("Or press \"x\" folowed by enter to return to the main menu:\n");
            for (int i = 0; i < _existingHabits.Count; i++)
            {
                ValidOptions.Add((i + 1).ToString());
                stringBuilder.Append($"{i + 1}. Drink {_existingHabits[i].Quantity} cups of {_existingHabits[i].Description} on {_existingHabits[i].Date}\n");
            }
        }
        MenuText = stringBuilder.ToString();
    }

    private void GetActionText()
    {
        Console.Clear();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"Please select one of the following actions: \n");
        stringBuilder.Append($"Type \"e\" followed by enter to add edit this habit\n");
        stringBuilder.Append($"Type \"d\" followed by enter to delete this habit\n");
        stringBuilder.Append($"Type \"x\" followed by enter to return to the main menu\n");
        MenuText = stringBuilder.ToString();

        ValidOptions.Add("e");
        ValidOptions.Add("d");
    }

    protected override void GetNextMenu(string selectedMenuOption, string action = "")
    {
        switch (true)
        {
            case bool _ when int.TryParse(selectedMenuOption, out int parseInt):
                Habit toUpdate = _existingHabits[parseInt - 1];
                if (action is "d")
                {
                    _database.Delete(toUpdate);
                    Console.WriteLine("Habit deleted Press any key to return to the main menu.");
                    Console.ReadKey();
                }
                else
                {
                    _newHabitMenu.Run(toUpdate);
                }
                break;
            case bool _ when selectedMenuOption is "x":
                break;
            default:
                Run();
                break;
        }
    }
}
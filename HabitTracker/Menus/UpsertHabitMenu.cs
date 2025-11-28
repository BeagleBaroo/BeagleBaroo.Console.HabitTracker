namespace HabitTracker.Menus;

public class UpsertHabitMenu : AbstractMenu
{
    private Habit? _habit;
    private Database _database;
    public UpsertHabitMenu(Database database)
    {
        _database = database;
    }

    public override void Run(Habit? habit = null)
    {
        _habit = habit ?? new Habit();

        if (_habit.Id is 0)
        {
            GetMenuText("habitType");
            PrintMenuText();
        }

        string userInput = string.Empty;
        if (userInput is not "x")
        {
            if (_habit.Id is 0)
            {
                ValidOptions.Add("w");
                ValidOptions.Add("x");
                userInput = GetUserInput(ValidOptions);
                _habit.Description = "water";
            }
            else
            {
                _habit.Description = "w";
            }

            if (userInput is not "x")
            {
                GetMenuText("quantity");
                PrintMenuText();
                bool validInput = false;
                int parseInt = 0;
                while (validInput is false)
                {
                    ValidOptions.Remove("w");
                    userInput = GetUserInput();

                    if (ValidOptions.Contains(userInput) || int.TryParse(userInput, out parseInt))
                    {
                        validInput = true;
                    }
                }

                if (userInput is not "x")
                {
                    _habit.Quantity = parseInt;

                    GetMenuText("date");
                    PrintMenuText();
                    validInput = false;
                    DateTime parseDateTime = DateTime.MinValue;
                    while (validInput is false)
                    {
                        ValidOptions.Add("t");
                        userInput = GetUserInput();

                        if (ValidOptions.Contains(userInput) || DateTime.TryParse(userInput, out parseDateTime))
                        {
                            validInput = true;
                        }
                    }

                    if (userInput is not "x")
                    {
                        if (userInput is "t")
                        {
                            parseDateTime = DateTime.UtcNow;
                        }

                        _habit.Date = parseDateTime.Date.ToString("ddd dd MMM yyyy");
                        _database.Upsert(_habit);
                    }
                }
            }

            string returToMainMenu = $"Habit {(_habit.Id is 0 ? "added" : "updated")} Press any key to return to the main menu.";
            Console.WriteLine(returToMainMenu);
            Console.ReadKey();
        }
    }

    protected override void GetMenuText(string option = "")
    {
        Console.Clear();
        StringBuilder stringBuilder = new StringBuilder();

        switch (option)
        {
            case "habitType":
                HabitDescriptionText(stringBuilder);
                break;
            case "quantity":
                QuantityDescriptionText(stringBuilder);
                break;
            case "date":
                DateDescriptionText(stringBuilder);
                break;
        }

        MenuText = stringBuilder.ToString();
    }

    private void HabitDescriptionText(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"Please select one of the following activities for your new habit log:\n");
        stringBuilder.Append($"Type \"w\" followed by enter to add a log for drinking glasses of water\n");
        stringBuilder.Append($"Type \"x\" followed by enter to return to the main menu\n");
    }

    private void QuantityDescriptionText(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"Please type the number of glasses of water you drank followed by enter\n");
        stringBuilder.Append($"Or type \"x\" followed by enter to return to the main menu\n");
    }

    private void DateDescriptionText(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"Please type one of the following to add a date for your new habit log:\n");
        stringBuilder.Append($"Type \"t\" followed by enter for todays date\n");
        stringBuilder.Append($"Type a date in the format \"dd/mm/yyyy\" followed by enter\n");
        stringBuilder.Append($"Type \"x\" followed by enter to return to the main menu\n");
    }

    protected override void GetNextMenu(string selectedMenuOption)
    {
        throw new NotImplementedException();
    }
}
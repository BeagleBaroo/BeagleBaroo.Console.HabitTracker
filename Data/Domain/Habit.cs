namespace HabitTracker.Data;

public class Habit
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public string? Date { get; set; }
}
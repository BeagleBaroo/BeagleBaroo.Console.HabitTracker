namespace HabitTracker.Data;

public class Database
{
    JsonWriter? jsonWriter;
    public Database()
    {
        InitialiseLogging();
        _dbFilePath = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}HabitTracker.sqlite";
        _connectionString = $"Data Source={_dbFilePath};";
        InitialiseSqlite();
    }

    private string _connectionString;
    private string _dbFilePath;

    private void InitialiseLogging()
    {
        StreamWriter logFile = File.CreateText("databaselog.json");
        logFile.AutoFlush = true;

        jsonWriter = new JsonTextWriter(logFile);
        jsonWriter.WriteStartObject();
        jsonWriter.WritePropertyName("DatabaseExceptions");
        jsonWriter.WriteStartArray();
    }

    private void InitialiseSqlite()
    {
        if (File.Exists(_dbFilePath) is false)
        {
            string createString = @"DROP TABLE IF EXISTS Habit; CREATE TABLE habits (habit_id integer [PRIMARY KEY], habit_text [NOT NULL], quantity integer [NOT NULL], date_of_habit text [NOT NULL]);";
            // Create the DB if it does not exist
            using (SqliteConnection sqliteConnection = new SqliteConnection(_connectionString))
            {
                SqliteCommand sqliteCommand = new(createString, sqliteConnection);
                try
                {
                    sqliteConnection.Open();
                    using (var command = sqliteConnection.CreateCommand())
                    {
                        command.CommandText = createString;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorToLog(ex);
                }
            }
        }
    }

    private void WriteErrorToLog(Exception ex)
    {
        if (jsonWriter is not null)
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("Exception");
            jsonWriter.WriteValue(ex.Message);
            jsonWriter.WriteEndObject();
        }
    }

    public void Dispose()
    {
        if (jsonWriter is not null)
        {
            jsonWriter.WriteEndArray();
            jsonWriter.WriteEndObject();
            jsonWriter.Close();
        }
    }
}
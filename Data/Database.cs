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
            string createDatabaseString = @"CREATE TABLE habits (id INTEGER PRIMARY KEY, description NOT NULL, quantity integer NOT NULL, date text NOT NULL);";
            // Create the DB if it does not exist
            SqliteCommand sqliteCommand = new SqliteCommand(createDatabaseString);
            ExecuteNonQuery(sqliteCommand);
        }
    }

    public void Create(Habit habit)
    {
        string createString = "INSERT INTO habits (description, quantity, date) VALUES (@description, @quantity, @date)";

        SqliteCommand sqliteCommand = new SqliteCommand(createString);
        sqliteCommand.Parameters.AddWithValue("@description", habit.Description);
        sqliteCommand.Parameters.AddWithValue("@quantity", habit.Quantity);
        sqliteCommand.Parameters.AddWithValue("@date", habit.Date);

        ExecuteNonQuery(sqliteCommand);
    }

    public List<Habit> Read()
    {
        List<Habit> habits = new List<Habit>();
        using (SqliteConnection sqliteConnection = new SqliteConnection(_connectionString))
        {
            string readString = "SELECT * FROM Habit";
            SqliteCommand sqliteCommand = new SqliteCommand(readString, sqliteConnection);
            try
            {
                sqliteConnection.Open();
                using (var reader = sqliteCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        habits.Add(new Habit
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Quantity = reader.GetInt32(2),
                            Date = reader.GetString(3)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorToLog(ex);
            }
        }
        return habits;
    }

    public void Update(Habit habit)
    {
        string updateString = "UPDATE habits SET description = @description, quantity = @quantity, date = @date WHERE id = @Id";

        SqliteCommand sqliteCommand = new SqliteCommand(updateString);
        sqliteCommand.Parameters.AddWithValue("@description", habit.Description);
        sqliteCommand.Parameters.AddWithValue("@quantity", habit.Quantity);
        sqliteCommand.Parameters.AddWithValue("@date", habit.Date);
        sqliteCommand.Parameters.AddWithValue("@Id", habit.Id);

        ExecuteNonQuery(sqliteCommand);
    }

    public void Delete(Habit habit)
    {
        string deleteString = "DELETE Habit WHERE id = @Id";

        SqliteCommand sqliteCommand = new SqliteCommand(deleteString);
        sqliteCommand.Parameters.AddWithValue("@Id", habit.Id);

        ExecuteNonQuery(sqliteCommand);
    }

    private void ExecuteNonQuery(SqliteCommand sqliteCommand)
    {
        if (string.IsNullOrWhiteSpace(sqliteCommand.CommandText) is false)
        {
            using (SqliteConnection sqliteConnection = new SqliteConnection(_connectionString))
            {
                sqliteCommand.Connection = sqliteConnection;
                try
                {
                    sqliteConnection.Open();
                    sqliteCommand.ExecuteNonQuery();
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
using MySql.Data.MySqlClient;


namespace Kitbox_project.DataBase
{
    /// <summary>Provides methods for interacting with the KitBoxing database, allowing for the addition, deletion, updating, and retrieval of data
    /// <list type="bullet">
    /// <item> <description>To add new data to a table, use <c>Add(Dictionary<string, object> data)</c>. This method inserts new records with the specified column values.</description> </item>
    /// <item> <description>To delete data from a table, use <c>Delete(Dictionary<string, object> conditions)</c>. This method deletes records that match the specified conditions.</description> </item>
    /// <item> <description>To update existing data in a table, use <c>Update(Dictionary<string, object> newData, Dictionary<string, object> conditions)</c>. This method updates records that match the specified conditions with new values.</description> </item>
    /// <item> <description>To retrieve data from a table based on specific conditions, use <c>GetData(Dictionary<string, object> conditions)</c>. This method returns a list of dictionaries, each representing a row of data that matches the conditions.</description> </item>
    /// </list>
    /// </summary>
    public class Database
    {
        protected string? tablename;
        public string Tablename { get => tablename; }

        public const string connectionString = "Server= pat.infolab.ecam.be ; port=63417;Database=KitBoxing;User ID=kitboxer;Password=kitboxing;";

        public void Add(Dictionary<string, object> data)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string columns = string.Join(", ", data.Keys);
                string values = string.Join(", ", data.Keys.Select(key => "@" + key));

                string query = $"INSERT IGNORE INTO {tablename} ({columns}) VALUES ({values})";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var entry in data)
                    {
                        command.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    command.ExecuteNonQuery();
                }
            }
        }
        public void Delete(Dictionary<string, object> conditions)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string whereClause = string.Join(" AND ", conditions.Keys.Select(key => $"{key} = @{key}"));

                string query = $"DELETE FROM {tablename} WHERE {whereClause}";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var condition in conditions)
                    {
                        command.Parameters.AddWithValue("@" + condition.Key, condition.Value);
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Dictionary<string, object> newData, Dictionary<string, object> conditions)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string setClause = string.Join(", ", newData.Keys.Select(key => $"{key} = @{key}"));
                string whereClause = string.Join(" AND ", conditions.Keys.Select(key => $"{key} = @{key}"));

                string query = $"UPDATE {tablename} SET {setClause} WHERE {whereClause}";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var entry in newData)
                    {
                        command.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    foreach (var condition in conditions)
                    {
                        command.Parameters.AddWithValue("@" + condition.Key, condition.Value);
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Dictionary<string, string>> GetData(Dictionary<string, string> conditions, List<string> columnsParameter = null)
        {
            List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

            if (columnsParameter == null) //optional parameter
            {
                columnsParameter = new List<string> { "*" };
            }

            string columns = string.Join(", ", columnsParameter);

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                // Create WHERE clause for specifying conditions for the SELECT query
                string whereClause = string.Join(" AND ", conditions.Keys.Select(key => $"{key}=@{key}"));

                // Construct the SQL SELECT query
                string query = $"SELECT {columns} FROM {Tablename} WHERE {whereClause}";

                // Create a MySqlCommand with the constructed query and the database connection
                MySqlCommand command = new MySqlCommand(query, connection);

                // Add parameters for the conditions in the WHERE clause
                foreach (var condition in conditions)
                {
                    command.Parameters.AddWithValue("@" + condition.Key, condition.Value);
                }

                // Create a dictionary to store the result
                Dictionary<string, string> resultData = new Dictionary<string, string>();

                MySqlDataReader reader = command.ExecuteReader();

                // Execute the SELECT query
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object columnValue = reader[columnName];
                        resultData.Add(columnName, columnValue.ToString());

                    }
                    dataList.Add(resultData);
                    resultData = new Dictionary<string, string>(); // Reset the dictionary for the next record
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return dataList.Count > 0 ? dataList : null; // Return null if no records found

        }

        public List<Dictionary<string, string>> LoadAll(List<string> columnsParameter = null)
        {
            List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

            if (columnsParameter == null) //optional parameter
            {
                columnsParameter = new List<string> { "*" };
            }

            string columns = string.Join(", ", columnsParameter);

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                // Construct the SQL SELECT query
                string query = $"SELECT {columns} FROM {Tablename}";

                // Create a MySqlCommand with the constructed query and the database connection
                MySqlCommand command = new MySqlCommand(query, connection);

                // Create a dictionary to store the result
                Dictionary<string, string> resultData = new Dictionary<string, string>();

                MySqlDataReader reader = command.ExecuteReader();

                // Execute the SELECT query
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object columnValue = reader[columnName];
                        resultData.Add(columnName, columnValue.ToString());

                    }
                    dataList.Add(resultData);
                    resultData = new Dictionary<string, string>(); // Reset the dictionary for the next record
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return dataList.Count > 0 ? dataList : null; // Return null if no records found

        }

    }
}

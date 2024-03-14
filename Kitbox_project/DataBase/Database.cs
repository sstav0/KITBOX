using MySql.Data.MySqlClient;


namespace Kitbox_project.DataBase
{
    

    public class Database
    {

        public Database(string id, string password){
        ID = id;
        Psswrd = password;
    }
        
        protected static  string ID =  Login.login ;
        
        protected  static string Psswrd =  Password.password;
        protected string tablename;
        public static string connectionString
        {
            get
            {
                return $"Server=pat.infolab.ecam.be;port=63417;Database=KitBoxing;User ID={ID};Password={Psswrd};";
            }
        }

        /// <summary>
        /// Inserts a new row into the table with the specified data.
        /// </summary>
        /// <param name="data">A dictionary representing the data to be inserted. The keys correspond to the column names, and the values are the data to be inserted into those columns. Example: <code>{{"Width", 52}, {"Height", 200}}</code> would insert a row with a width of 52 and a height of 200.</param>
        /// <remarks>This method constructs an INSERT IGNORE INTO query, which means it will ignore the insert operation for rows that would cause a duplicate entry in a unique index or primary key.<code>string query = $"INSERT IGNORE INTO {tablename} ({columns}) VALUES ({values})";</code></remarks>
        public async Task Add(Dictionary<string, object> data)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string columns = string.Join(", ", data.Keys);
                string values = string.Join(", ", data.Keys.Select(key => "@" + key));

                string query = $"INSERT IGNORE INTO {tablename} ({columns}) VALUES ({values})";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var entry in data)
                    {
                        command.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Deletes rows from the table that match the specified conditions.
        /// </summary>
        /// <param name="conditions">A dictionary representing the conditions that rows must meet to be deleted. The keys correspond to the column names, and the values are the conditions that the columns must satisfy. Example: <code>{{"Width", 52}, {"Material", "Wood"}}</code> would delete rows where the width is 52 and the material is wood.</param>
        /// <remarks>This method constructs a DELETE FROM query using the conditions to form a WHERE clause, specifying which rows should be removed.<code>string query = $"DELETE FROM {tablename} WHERE {whereClause}";</code></remarks>
        public async Task Delete(Dictionary<string, object> conditions)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string whereClause = string.Join(" AND ", conditions.Keys.Select(key => $"{key} = @{key}"));

                string query = $"DELETE FROM {tablename} WHERE {whereClause}";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var condition in conditions)
                    {
                        command.Parameters.AddWithValue("@" + condition.Key, condition.Value);
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Updates the rows in the table that match the specified conditions with the new data provided.
        /// </summary>
        /// <param name="newData">A dictionary representing the new data to be updated. The keys correspond to the column names, and the values are the new data to be updated in those columns. Example: <code>{{"Height", 210}}</code> would update the height column to 210 for all rows matching the conditions.</param>
        /// <param name="conditions">A dictionary representing the conditions that rows must meet to be updated. The keys correspond to the column names, and the values are the conditions that the columns must satisfy. Example: <code>{{"Width", 52}}</code> would update rows where the width is 52.</param>
        /// <remarks>This method constructs an UPDATE query using the newData to form a SET clause and the conditions to form a WHERE clause, specifying which rows should be updated and how. <code>string query = $"UPDATE {tablename} SET {setClause} WHERE {whereClause}";</code></remarks>
        public async Task Update(Dictionary<string, object> newData, Dictionary<string, object> conditions)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

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

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// This function takes a dictionary of conditions and a list of columns to retrieve from the database.
        /// </summary>
        /// <param name="conditions">Example : <code>{{"Width", 52}, {"Reference", "Door"}}</code>Means that the function will return every row that has a "Reference" (=column name) "Door"(=column value) and a "Width" "52"</param>
        /// <param name="columnsParameter">This parameter indicates which columns will be selected. Example : <code>{"Reference", "Door_color"}</code>Means that the function will only return these column from the result of the query.</param>
        /// <returns>A List of rows that are formated as dictionary. Example : <code>{{{"Reference", "Door"}, {"Door_color", Brown}}, {{"Reference", "Door"}, {"Door_color", "White"}}}</code></returns>
        public async Task<List<Dictionary<string, string>>> GetData(Dictionary<string, string> conditions, List<string> columnsParameter = null)
        {
            List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

            if (columnsParameter == null) //optional parameter
            {
                columnsParameter = new List<string> { "*" };
            }

            string columns = string.Join(", ", columnsParameter);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Create WHERE clause for specifying conditions for the SELECT query
                    string whereClause = string.Join(" AND ", conditions.Keys.Select(key => $"{key}=@{key}"));

                    // Construct the SQL SELECT query
                    string query = $"SELECT {columns} FROM {tablename} WHERE {whereClause}";

                    // Create a MySqlCommand with the constructed query and the database connection
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters for the conditions in the WHERE clause
                        foreach (var condition in conditions)
                        {
                            command.Parameters.AddWithValue("@" + condition.Key, condition.Value);
                        }

                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            // Execute the SELECT query
                            while (await reader.ReadAsync())
                            {
                                Dictionary<string, string> resultData = new Dictionary<string, string>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue = reader[columnName];
                                    resultData.Add(columnName, columnValue.ToString());
                                }

                                dataList.Add(resultData);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return dataList.Count > 0 ? dataList : null; // Return null if no records found
        }

        /// <summary>
        /// Retrieves all rows from the table, optionally limiting the columns returned.
        /// </summary>
        /// <param name="columnsParameter">An optional list of column names to be selected. If not provided, all columns (*) are selected. Example: <code>{"Reference", "Panel_color"}</code> specifies that only the 'Reference' and 'Panel_color' columns should be returned from the query.</param>
        /// <returns>A list of dictionaries, where each dictionary represents a row from the table with the specified columns. Each dictionary's key corresponds to a column name, and its value is the data for that column in the row. If no rows are found, null is returned.</returns>
        /// <remarks>This method constructs a SELECT query that can be customized with specific columns to retrieve. It is particularly useful for loading entire tables or large portions of data without specifying conditions. <code>string query = $"SELECT {columns} FROM {tablename}";</code></remarks>
        public async Task<List<Dictionary<string, string>>> LoadAll(List<string> columnsParameter = null)
        {
            List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

            if (columnsParameter == null) // optional parameter
            {
                columnsParameter = new List<string> { "*" };
            }

            string columns = string.Join(", ", columnsParameter);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Construct the SQL SELECT query
                    string query = $"SELECT {columns} FROM {tablename}";

                    // Create a MySqlCommand with the constructed query and the database connection
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            // Execute the SELECT query
                            while (await reader.ReadAsync())
                            {
                                Dictionary<string, string> resultData = new Dictionary<string, string>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue = reader[columnName];
                                    resultData.Add(columnName, columnValue.ToString());
                                }

                                dataList.Add(resultData);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return dataList.Count > 0 ? dataList : null; // Return null if no records found
        }

    }
}

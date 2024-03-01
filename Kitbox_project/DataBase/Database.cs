using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using MySql.Data.MySqlClient;

namespace Kitbox_project.DataBase
{
    public class Database
    {
        protected string tablename;
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

        public List<Dictionary<string, object>> GetData(Dictionary<string, object> conditions)
        {
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>(); 

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create WHERE clause for specifying conditions for the SELECT query
                string whereClause = string.Join(" AND ", conditions.Keys.Select(key => $"{key} = @{key}"));

                // Construct the SQL SELECT query
                string query = $"SELECT * FROM {tablename} WHERE {whereClause}";

                // Create a MySqlCommand with the constructed query and the database connection
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters for the conditions in the WHERE clause
                    foreach (var condition in conditions)
                    {
                        command.Parameters.AddWithValue("@" + condition.Key, condition.Value);
                    }

                    // Create a dictionary to store the result
                    Dictionary<string, object> resultData = new Dictionary<string, object>();

                    // Execute the SELECT query
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object columnValue = reader.GetValue(i);
                                resultData.Add(columnName, columnValue);
                            }

                            dataList.Add(resultData);
                        }
                    }

                    return dataList.Count > 0 ? dataList : null; // Return null if no records found
                }
            }
        }
    }
}

namespace Kitbox_project;

using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;
public class DatabaseCatalog : Database
{
    
    
        public List<Dictionary<string, object>> GetByReference(string Reference)
        {
            List<Dictionary<string, object>> catalogDataList = new List<Dictionary<string, object>>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Catalog WHERE Reference = @Reference";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Reference", Reference);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> catalogData = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object columnValue = reader.GetValue(i);
                                catalogData.Add(columnName, columnValue);
                            }

                            catalogDataList.Add(catalogData);
                        }
                    }
                }
            }

            // Return the list of dictionaries
            return catalogDataList;
        }

}


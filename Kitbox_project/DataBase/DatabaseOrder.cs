namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;

public class DatabaseOrder : Database
{
 public DatabaseOrder(string id, string password){

    tablename = "Order";
    ID = id;
    Password = password;
    }

    public List<object> LoadAll()
    {
        List<object> l = new List<object>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = $"SELECT * FROM {tablename}";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {

                // Execute the SELECT query
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            //to implement
                        }
                    }
                }
            }
            return l;

        }

    }
}

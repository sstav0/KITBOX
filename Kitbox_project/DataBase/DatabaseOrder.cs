namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;

public class DatabaseOrder : Database
{
    DatabaseStock databaseStock = new DatabaseStock("storekeeper", "storekeeper");
    public DatabaseOrder(string id, string password) : base(id, password)
    {

        tablename = "Order";
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

    public async Task<bool> NotifyUnavailability(string idOrder) // /!\ notify unavailability for a locker and not the complete order
    {
        Dictionary<string, string> orderIdList = new Dictionary<string, string>
        {
            {"orderId", idOrder}
        };

        var partsOrdered = await GetData(orderIdList);

        Dictionary<string, string> stockRefList = new Dictionary<string, string>();
        foreach (var part in partsOrdered)
        {
            if (part.ContainsKey("Reference"))
            {
                stockRefList["Reference"] = part["Reference"];
            }
        }

        var partsStocked = await databaseStock.GetData(stockRefList);

        // Process partsStocked as needed

        return true;
    }
}


namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
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
        Dictionary<string, string> orderIdDict = new Dictionary<string, string> //GetData() parameter
        {
            {"orderId", idOrder}
        };
        List<string> conditionColumn = new List<string>() { "Reference", "Quantity"}; //GetData() parameter

        List<Dictionary<string,string>> partsOrdered = await GetData(orderIdDict,conditionColumn); // List<Dict<string, string>> every data from DB for the given idOrder  

        Console.WriteLine(partsOrdered.Count);

        Dictionary<string, string> stockPartsDict = new Dictionary<string, string>(); //GetData() parameter
        foreach(string columns in partsOrdered[0].Values)
        {
            stockPartsDict["Reference"] = columns;
            List<Dictionary<string, string>> isStocked = await databaseStock.GetData(stockPartsDict, conditionColumn); // List<Dict<string, string>> every data from stock DB for the given idOrder

            Console.WriteLine(isStocked[0]["Reference"]);
        }
        
        return true;
    }
}


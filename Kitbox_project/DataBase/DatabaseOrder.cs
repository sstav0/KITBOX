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

        tablename = "OrderTable";
    }

     public static List<OrderItem> ConvertToOrderItem(List<Dictionary<string, string>> data)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        foreach (var item in data)
        {
            orderItems.Add(new OrderItem(
                int.Parse(item["idOrder"]),
                int.Parse(item["idCustomer"]),
,               int.Parse(item["Quantity"]),
                item["Code"],
                item["Date"]
                
                
            ));
        }
        return orderItems;
    }
}


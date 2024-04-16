namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using MySql.Data.MySqlClient;
public class DatabaseStock : Database
{

    public DatabaseStock(string id, string password):base(id, password){
        tablename = "Stock";

    }

    public static List<StockItem> ConvertToStockItem(List<Dictionary<string, string>> data)
    {
        List<StockItem> stockItems = new List<StockItem>();
        foreach (var item in data)
        {
            stockItems.Add(new StockItem(
                int.Parse(item["idStock"]),
                item["Reference"],
                item["Code"],
                int.Parse(item["Quantity"]),
                int.Parse(item["IncomingQuantity"]),
                int.Parse(item["OutgoingQuantity"]),
                bool.Parse(item["InCatalog"])
            ));
        }
        return stockItems;
    }
}

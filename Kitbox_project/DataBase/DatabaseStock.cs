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
                ParseStockBool(item["InCatalog"])
            ));
        }
        return stockItems;
    }

    public static List<OrderStockItem> ConvertToOrderStockItem(List<Dictionary<string, string>> data)
    {
        List<OrderStockItem> stockItems = new List<OrderStockItem>();
        foreach (var item in data)
        {
            stockItems.Add(new(
                int.Parse(item["idStock"]),
                item["Reference"],
                item["Code"],
                int.Parse(item["Quantity"]),
                int.Parse(item["IncomingQuantity"]),
                int.Parse(item["OutgoingQuantity"]),
                ParseStockBool(item["InCatalog"])
            ));
        }
        return stockItems;
    }

    public static Dictionary<string, object> ConvertFromStockItem(StockItem stockItem)
    {
        Dictionary<string, object> DBStockItem = new();
        if (stockItem.Id is not null)
        {
            DBStockItem.Add("idStock", stockItem.Id);            
        }
        DBStockItem.Add("Reference", stockItem.Reference);
        DBStockItem.Add("Code", stockItem.Code );
        DBStockItem.Add("Quantity", stockItem.Quantity );
        DBStockItem.Add("IncomingQuantity", stockItem.IncomingQuantity );
        DBStockItem.Add("OutgoingQuantity", stockItem.OutgoingQuantity );
        DBStockItem.Add("InCatalog", ParseStockBool(stockItem.InCatalog) );

        return DBStockItem;
    }

    private static bool ParseStockBool(string str)
    {
        if (string.IsNullOrEmpty(str) || str is "0")
        {
            return false;
        }
        return true;
    }

    private static string ParseStockBool(bool itemBool)
    { 
        if(itemBool)
        {
            return "0";
        }
        return "1";
    }
}

﻿namespace Kitbox_project;

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

    private static bool ParseStockBool(string str)
    {
        if (string.IsNullOrEmpty(str) || str is "0")
        {
            return false;
        }
        return true;
    }
}

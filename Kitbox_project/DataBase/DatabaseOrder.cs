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
}


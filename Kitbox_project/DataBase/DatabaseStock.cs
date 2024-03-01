namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using MySql.Data.MySqlClient;
public class DatabaseStock : Database
{
    public DatabaseStock(string id, string password){
        tablename = "Stock";
        ID = id;
        Password = password;
    }
}

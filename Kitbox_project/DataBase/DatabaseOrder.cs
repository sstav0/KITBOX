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
}

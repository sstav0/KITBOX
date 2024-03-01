namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;
public class DatabaseOrder : Database
{
 public DatabaseOrder(){

    tablename = "Order";
    }
}

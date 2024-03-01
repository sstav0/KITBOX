namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using MySql.Data.MySqlClient;
public class DatabaseCabinet : Database
{
    public DatabaseCabinet(string id, string password){
        tablename = "Cabinet";
        ID = id;
        Password = password;
    }
}

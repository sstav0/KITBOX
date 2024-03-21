namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using MySql.Data.MySqlClient;
public class DatabaseSuppliers : Database
{

    public DatabaseSuppliers(string id, string password) : base(id, password)
    {
        tablename = "Suppliers";

    }
}

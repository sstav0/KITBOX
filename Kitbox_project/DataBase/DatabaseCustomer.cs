﻿namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;
public class DatabaseCustomer : Database
{

    public DatabaseCustomer(string id, string password):base(id, password){
    tablename = "Customer";

    }
}

﻿namespace Kitbox_project;

using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;
public class DatabaseCatalog : Database
{
    public DatabaseCatalog(string id, string password){
    tablename = "Catalog";
    ID = id;
    Password = password;
    }

    public List<Dictionary<string, object>> GetByReference(string reference)
    {
        var catalogData = new Dictionary<string, object>
        {
            {"Reference", reference}
        };

        return GetData(catalogData);
    }


}


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

    public static List<Supplier> ConvertToSupplier(List<Dictionary<string, string>> data)
    {
        List<Supplier> suppliers = new List<Supplier>();   
        foreach (var supplier in data)
        {
            suppliers.Add(new Supplier(
                            int.Parse(supplier["idSuppliers"]),
                            supplier["NameofSuppliers"],
                            supplier["Address"],
                            supplier["City"],
                            int.Parse(supplier["Postal Code"]),
                            supplier["Country"]
                            ));
        }

        return suppliers;
    }
}

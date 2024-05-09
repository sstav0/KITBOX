using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitbox_project.Models;

namespace Kitbox_project.DataBase;
public class DatabaseCatalogPrices : Database
{
    public DatabaseCatalogPrices(string id, string password) : base(id, password)
    {
        tablename = "Catalog";

    }

    public static List<CatalogPriceItem> ConvertToPriceItem(List<Dictionary<string, string>> data)
    {
        List<CatalogPriceItem> priceItems = new List<CatalogPriceItem>();
        foreach (var item in data)
        {
            priceItems.Add(new CatalogPriceItem(
                item["Code"],
                double.Parse(item["Price"])
            ));
        }
        return priceItems;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitbox_project.Models;

namespace Kitbox_project.DataBase;
public class DatabasePrices : Database
{
    public DatabasePrices(string id, string password) : base(id, password)
    {
        tablename = "Prices";

    }

    public static List<PriceItem> ConvertToPriceItem(List<Dictionary<string, string>> data)
    {
        List<PriceItem> priceItems = new List<PriceItem>();
        foreach (var item in data)
        {
            priceItems.Add(new PriceItem(
                item["Supplier"],
                item["ItemCode"],
                double.Parse(item["Price"])
            ));
        }
        return priceItems;
    }
}

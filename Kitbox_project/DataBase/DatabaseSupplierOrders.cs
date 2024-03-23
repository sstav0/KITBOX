using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitbox_project.Models;
using Kitbox_project.DataBase;

namespace Kitbox_project
{
    public class DatabaseSupplierOrders : Database
    {
        public DatabaseSupplierOrders(string id, string password) : base(id, password)
        {
            tablename = "SupplierOrder";
        }

        public static async Task<List<SupplierOrder>> ConvertToSupplierOrder(List<Dictionary<string, string>> data)
        {
            DatabaseStock DBStock = new DatabaseStock("kitboxer", "kitboxing");

            List<SupplierOrder> supplierOrders = new List<SupplierOrder>();
            foreach (var order in data)
            {
                var item = await DBStock.GetData(new Dictionary<string, string> { { "Code", order["itemCode"] } });

                supplierOrders.Add(new SupplierOrder(
                    int.Parse(order["idSupplierOrder"]),
                    DatabaseStock.ConvertToStockItem(item)[0],
                    int.Parse(order["idSupplier"]),
                    int.Parse(order["delay"]),
                    int.Parse(order["quantity"]),
                    double.Parse(order["price"]),
                    order["status"]
                ));
            }
            return supplierOrders;
        }
    }
}

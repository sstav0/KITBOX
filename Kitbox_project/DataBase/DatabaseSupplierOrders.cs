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

        public static List<SupplierOrder> ConvertToSupplierOrder(List<Dictionary<string, string>> data)
        {
            List<SupplierOrder> supplierOrders = new List<SupplierOrder>();
            foreach (var order in data)
            {
                supplierOrders.Add(new SupplierOrder(
                    int.Parse(order["idSupplierOrder"]),
                    int.Parse(order["idSupplier"]),
                    order["deliveryDate"],
                    double.Parse(order["price"]),
                    order["status"]
                ));
            }
            return supplierOrders;
        }
    }
}

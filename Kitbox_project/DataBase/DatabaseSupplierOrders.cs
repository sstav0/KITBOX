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
            tablename = "SupplierOrders";
        }

        public static List<SupplierOrder> ConvertToSupplierOrder(List<Dictionary<string, string>> data)
        {
            List<SupplierOrder> supplierOrders = new List<SupplierOrder>();
            foreach (var item in data)
            {
                //supplierOrders.Add(new SupplierOrder(
                //    int.Parse(item["idStock"]),
                //    item["Reference"],
                //    item["Code"],
                //    int.Parse(item["Quantity"])
                //));
            }
            return supplierOrders;
        }
    }
}

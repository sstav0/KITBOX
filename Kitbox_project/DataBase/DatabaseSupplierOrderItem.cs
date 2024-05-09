using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.DataBase
{
    internal class DatabaseSupplierOrderItem : Database
    {
        public DatabaseSupplierOrderItem(string id, string password) : base(id, password)
        {
            tablename = "SupplierOrderItem";
        }
    }
}

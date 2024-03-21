using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class SupplierOrder
    {
        private int _orderID;
        private StockItem _item;
        private string _supplier;
        private DateTime _date;
        private double _price;
        private string _status;
        public SupplierOrder(int orderID, StockItem item, string supplier, DateTime date, double price, string status)
        {
            _orderID = orderID;
            _item = item;
            _supplier = supplier;
            _date = date;
            _price = price;
            _status = status;
        }
        public int OrderID
        {
            get => _orderID;
            set => _orderID = value;
        }

        public StockItem Item
        {
            get => _item;
            set => _item = value;
        }

        public string Supplier
        {
            get => _supplier;
            set => _supplier = value;
        }
        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }
        public double Price
        {
            get => _price;
            set => _price = value;
        }
        public string Status
        {
            get => _status;
            set => _status = value;
        }
    }
}

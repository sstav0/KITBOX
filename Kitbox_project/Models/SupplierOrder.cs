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
        private List<SupplierOrderItem> _supplierOrderItems;
        private int _supplierId;
        private string _deliveryDate;
        private double _price;
        private string _status;
        public SupplierOrder(int orderID, int supplierId, string deliveryDate, double price, string status)
        {
            _orderID = orderID;
            _supplierId = supplierId;
            _deliveryDate = deliveryDate;
            _price = price;
            _status = status;
            _supplierOrderItems = new List<SupplierOrderItem>();
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

        public List<SupplierOrderItem> SupplierOrderItems
        {
            get => _supplierOrderItems;
            set => _supplierOrderItems = value;
        }

        public int SupplierId
        {
            get => _supplierId;
            set => _supplierId = value;
        }
        public string DeliveryDate
        {
            get => _deliveryDate;
            set => _deliveryDate = value;
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

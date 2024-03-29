﻿using System;
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
        private int _supplierId;
        private int _delay;
        private int _quantity;
        private double _price;
        private string _status;
        public SupplierOrder(int orderID, StockItem item, int supplierId, int delay, int quantity, double price, string status)
        {
            _orderID = orderID;
            _item = item;
            _supplierId = supplierId;
            _delay = delay;
            _quantity = quantity;
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

        public int SupplierId
        {
            get => _supplierId;
            set => _supplierId = value;
        }
        public int Delay
        {
            get => _delay;
            set => _delay = value;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
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

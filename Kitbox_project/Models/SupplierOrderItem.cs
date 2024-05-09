using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class SupplierOrderItem
    {
        private string _reference;
        private string _code;
        private int _quantity;
        private double _unitPrice;
        private double _totalPrice;

        public SupplierOrderItem(string reference, string code, int quantity, double unitPrice)
        {
            Reference = reference;
            Code = code;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = Math.Round(Quantity * UnitPrice, 2);
        }

        public string Reference
        {
            get => _reference;
            set => _reference = value;
        }

        public string Code
        {
            get => _code;
            set => _code = value;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }

        public double UnitPrice
        {
            get => _unitPrice;
            set => _unitPrice = value;
        }

        public double TotalPrice
        {
            get => _totalPrice;
            set => _totalPrice = value;
        }
    }
}

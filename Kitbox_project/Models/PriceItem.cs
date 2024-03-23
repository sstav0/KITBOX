using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class PriceItem : INotifyPropertyChanged
    {
        // Write documentation for the class PriceItem with description of constructor's parameters 
        /// <summary>
        /// Creates a price item with specified supplier and price.
        /// <list type="bullet">
        /// <item> <description>To <b>get</b> the Supplier of the price item use : <c>PriceItem.Supplier</c> </description> </item>
        /// <item> <description>To <b>get</b> the Code of the item behind the price use : <c>PriceItem.ItemCode</c> </description> </item>
        /// <item> <description>To <b>get</b> the Price of the price item use : <c>PriceItem.Price</c> </description> </item>
        /// </list>
        /// </summary>

        private string _supplier;
        private string _itemCode;
        private double _price;

        public PriceItem(string supplier, string itemCode, double price)
        {
            _supplier = supplier;
            _itemCode = itemCode;
            _price = price;
        }

        public string Supplier
        {
            get => _supplier;
            set
            {
                _supplier = value;
                OnPropertyChanged(nameof(Supplier));
            }
        }
        public string ItemCode
        {
            get => _itemCode;
            set
            {
                _itemCode = value;
                OnPropertyChanged(nameof(ItemCode));
            }
        }
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

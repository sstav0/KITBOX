using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    // Write documentation for the class StockItem with description of constructor's parameters 
    /// <summary>
    /// Creates a stock item with specified name, code, quantity, and dimensions.
    /// <list type="bullet">
    /// <item> <description>To <b>get</b> the ID of the stock item use : <c>StockItem.Id</c> </description> </item>
    /// <item> <description>To <b>get</b> the reference of the stock item use : <c>StockItem.Reference</c> </description> </item>
    /// <item> <description>To <b>get</b> the code of the stock item use : <c>StockItem.Code</c> </description> </item>
    /// <item> <description>To <b>get</b> the quantity of the stock item use : <c>StockItem.Quantity</c> </description> </item>
    /// <item> <description>To <b>set</b> the ID of the stock item, use : <c>StockItem.Id = int id</c>.</description> </item>
    /// <item> <description>To <b>set</b> the reference of the stock item, use : <c>StockItem.Reference = string reference</c>.</description> </item>
    /// <item> <description>To <b>set</b> the code of the stock item, use : <c>StockItem.Code = string code</c>.</description> </item>
    /// <item> <description>To <b>set</b> the quantity of the stock item, use : <c>StockItem.Quantity = int quantity</c>.</description> </item>
    /// </list>
    /// </summary>
    public class StockItem : INotifyPropertyChanged
    {
        private int _id;
        private string _reference;
        private string _code;
        private int _quantity;
        private int _incomingQuantity;
        private int _outgoingQuantity;
        private bool _inCatalog;

        public StockItem(int id, string reference, string code, int quantity, int incomingQuantity, int outgoingQuantity, bool inCatalog)
        {
            _id = id;
            _reference = reference;
            _code = code;
            _quantity = quantity;
            _incomingQuantity = incomingQuantity;
            _outgoingQuantity = outgoingQuantity;
            _inCatalog = inCatalog;
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Reference
        {
            get => _reference;
            set
            {
                _reference = value;
                OnPropertyChanged(nameof(Reference));
            }
        }

        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public int IncomingQuantity
        {
            get => _incomingQuantity;
            set
            {
                _incomingQuantity = value;
                OnPropertyChanged(nameof(IncomingQuantity));
            }
        }

        public int OutgoingQuantity
        {
            get => _outgoingQuantity;
            set
            {
                _outgoingQuantity = value;
                OnPropertyChanged(nameof(OutgoingQuantity));
            }
        }

        public bool InCatalog
        {
            get => _inCatalog;
            set
            {
                _inCatalog = value;
                OnPropertyChanged(nameof(InCatalog));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

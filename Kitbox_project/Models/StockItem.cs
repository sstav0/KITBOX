using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    // Write documentation for the class StockItem with description of constructor's parameters 
    /// <summary>
    /// Creates a stock item with specified name, code, quantity, and dimensions.
    /// <list type="bullet">
    /// <item> <description>To <b>get</b> the name of the stock item use : <c>StockItem.Name</c> </description> </item>
    /// <item> <description>To <b>get</b> the code of the stock item use : <c>StockItem.Code</c> </description> </item>
    /// <item> <description>To <b>get</b> the quantity of the stock item use : <c>StockItem.Quantity</c> </description> </item>
    /// <item> <description>To <b>get</b> the dimensions of the stock item use : <c>StockItem.Dimensions</c> </description> </item>
    /// <item> <description>To <b>set</b> the name of the stock item, use : <c>StockItem.Name = string name</c>.</description> </item>
    /// <item> <description>To <b>set</b> the code of the stock item, use : <c>StockItem.Code = string code</c>.</description> </item>
    /// <item> <description>To <b>set</b> the quantity of the stock item, use : <c>StockItem.Quantity = int quantity</c>.</description> </item>
    /// <item> <description>To <b>set</b> the dimensions of the stock item, use : <c>StockItem.Dimensions = string dimensions</c>.</description> </item>
    /// </list>
    /// </summary>
    /// <param name="name"> name of the stock item.</param>
    /// <param name="code"> code of the stock item.</param>
    /// <param name="quantity"> quantity of the stock item.</param>
    /// <param name="dimensions"> dimensions of the stock item.</param>
    public class StockItem
    {
        private string _name;
        private string _code;
        private int _quantity;
        private string _dimensions;

        public StockItem(string name, string code, int quantity, string dimensions)
        {
            this._name = name;
            this._code = code;
            this._quantity = quantity;
            this._dimensions = dimensions;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
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
        public string Dimensions
        {
            get => _dimensions;
            set => _dimensions = value;
        }
    }
}

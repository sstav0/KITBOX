using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    internal class StockViewModel
    {
        // StockData sould be List<StockItem> directly from database with a Model StockItem.cs. The data displayed are his public properties.
        private List<StockItem> _stockData;
        public List<StockItem> StockData { get => _stockData; set => _stockData = value; }

        public StockViewModel()
        {
            StockData = new List<StockItem>
            {
                new StockItem("Apple", "Ball", 5, "10x10"),
                new StockItem("Banana", "Line", 7, "2x15")
            };
        }
    }
}

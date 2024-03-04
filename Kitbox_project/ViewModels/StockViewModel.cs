using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    internal class StockViewModel : INotifyPropertyChanged
    {
        private List<StockItemViewModel> _stockData;
        public List<StockItemViewModel> StockData { get => _stockData; set => _stockData = value; }

        public StockViewModel()
        {
            // Simulating data from the database
            var stockItems = new List<StockItem>
            {
                new StockItem(1, "Item1","123", 10),
                new StockItem(2, "Item2","456", 20),
                // Add more items as needed
            };

            // Convert StockItem to StockItemViewModel
            StockData = new List<StockItemViewModel>(ConvertToViewModels(stockItems));
        }

        private IEnumerable<StockItemViewModel> ConvertToViewModels(IEnumerable<StockItem> stockItems)
        {
            return stockItems.Select(item => new StockItemViewModel(item.Id, item.Reference, item.Code, item.Quantity));
        }

        public void EditUpdateQuantity(StockItemViewModel stockItem)
        {
            if (stockItem.IsEditing)
            {
                // Update the quantity in the database using appropriate logic
                // Example: stockItem.Id is assumed to be a unique identifier for the item in the database
                // Implement your logic to update the quantity in the database
                // database.UpdateQuantity(stockItem.Id, stockItem.Quantity);

                stockItem.IsEditing = false;
                stockItem.ButtonText = "Edit";
            }
            else
            {
                stockItem.IsEditing = true;
                stockItem.ButtonText = "Update";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // ViewModel for stock items
    public class StockItemViewModel : StockItem, INotifyPropertyChanged
    {
        private bool _isEditing;
        private string _buttonText;

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public StockItemViewModel(int id, string reference, string code, int quantity) : base(id, reference, code, quantity)
        {
            IsEditing = false;
            ButtonText = "Edit";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

﻿using Kitbox_project.Models;
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
        public List<StockItemViewModel> StockData
        { 
            get => _stockData;
            set
            {
                _stockData = value;
                OnPropertyChanged(nameof(StockData));
            }
        }

        public StockViewModel()
        {
            // Simulating data from the database
            var stockItems = new List<StockItem>
            {
                new StockItem(1, "Item1","123", 10),
                new StockItem(2, "Item2","456", 20),
            };

            // Convert StockItem to StockItemViewModel
            StockData = new List<StockItemViewModel>(ConvertToViewModels(stockItems));
        }

        private static IEnumerable<StockItemViewModel> ConvertToViewModels(IEnumerable<StockItem> stockItems)
        {
            return stockItems.Select(item => new StockItemViewModel(item.Id, item.Reference, item.Code, item.Quantity));
        }

        public void EditUpdateQuantity(StockItemViewModel stockItem)
        {
            // If Update button pressed
            if (stockItem.IsEditing)
            {
                if (stockItem.TempQuantity < 0)
                {
                    stockItem.TempQuantity = stockItem.Quantity;
                    // Show error message : Quantity must be a positive number
                    return;
                }
                // Update the quantity in the database using appropriate logic
                // Example: stockItem.Id is assumed to be a unique identifier for the item in the database
                // Implement your logic to update the quantity in the database
                // database.UpdateQuantity(stockItem.Id, stockItem.Quantity);
                stockItem.Quantity = stockItem.TempQuantity;

                stockItem.IsEditing = false;
                stockItem.ButtonText = "Edit";
                stockItem.ButtonColor = Color.Parse("#512BD4");
            }
            // If Edit button pressed
            else
            {
                stockItem.IsEditing = true;
                stockItem.ButtonText = "Update";
                stockItem.ButtonColor = Color.Parse("green");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // ViewModel for stock items
    public class StockItemViewModel : StockItem
    {
        private bool _isEditing;
        private string _buttonText;
        private Color _buttonColor;
        private int _tempQuantity;

        public StockItemViewModel(int id, string reference, string code, int quantity) : base(id, reference, code, quantity)
        {
            IsEditing = false;
            ButtonText = "Edit";
            ButtonColor = Color.Parse("#512BD4");
            TempQuantity = quantity;
        }
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

        public Color ButtonColor
        {
            get => _buttonColor;
            set
            {
                _buttonColor = value;
                OnPropertyChanged(nameof(ButtonColor));
            }
        }

        public int TempQuantity
        {
            get => _tempQuantity;
            set
            {
                if (!IsEditing)
                {
                    _tempQuantity = value;
                    OnPropertyChanged(nameof(TempQuantity));
                }
            }
        }
    }
}
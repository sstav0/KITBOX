
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Kitbox_project.ViewModels.StockViewModel;

namespace Kitbox_project.ViewModels

    //Besoin d'être sûre de la forme des prix utilisés dans la DB avant de faire fonctionner ce code
{
    internal class PriceViewModel : INotifyPropertyChanged
    {
        private List<PriceItemViewModel> _pricesData;
        private DatabasePrices DBPrices = new DatabasePrices("kitboxer", "kitboxing");

        public PriceViewModel()
        {

        }
        private async void LoadDataAsync()
        {
            var stockItems = await DBPrices.LoadAll();
            PricesData = PriceItemViewModel.ConvertToViewModels(DatabasePrices.ConvertToPriceItem(stockItems));
        }

        public List<PriceItemViewModel> PricesData
        {
            get => _pricesData;
            set
            {
                _pricesData = value;
                OnPropertyChanged(nameof(PricesData));
            }
        }

        public void ApplyFilter(string searchText)
        {
            searchText = searchText.Trim();
            foreach (var item in PricesData)
            {
                // Filter items based on search text
                item.PriceItemVisibility =
                    string.IsNullOrWhiteSpace(searchText) ||
                    item.Supplier.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    item.ItemCode.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            }
        }

        public async Task EditUpdatePrice(PriceItemViewModel priceItem)
        {
            // If Update button pressed
            if (priceItem.IsEditing)
            {
                // If the input price is a number and non-negative
                if (priceItem.IsValidPrice)
                {
                    priceItem.InputPrice = priceItem.InputPrice.TrimStart('0') != "" ? priceItem.InputPrice.TrimStart('0') : "0";

                    //priceItem.Price = Convert.ToDouble(priceItem.InputPrice);
                    //await DBPrices.Update(
                    //    new Dictionary<string, object> { { "Price", priceItem.Price } },
                    //    new Dictionary<string, object> { { "Supplier", priceItem.Supplier } });

                    priceItem.IsEditing = false;
                    priceItem.ButtonText = "Edit";
                    priceItem.ButtonColor = Color.Parse("#512BD4");
                }
                // If the input quantity is not a number or negative, keep the previous quantity
                else
                {
                    priceItem.InputPrice = Convert.ToString(priceItem.Price);
                    priceItem.ButtonText = "Update";
                    priceItem.ButtonColor = Color.Parse("green");
                }
            }
            // If Edit button pressed
            else
            {
                priceItem.IsEditing = true;
                priceItem.ButtonText = "Update";
                priceItem.ButtonColor = Color.Parse("green");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public class PriceItemViewModel : PriceItem
        {
            private bool _isEditing;
            private string _buttonText;
            private Color _buttonColor;
            private string _inputPrice;
            private bool _isValidPrice;
            private bool _priceItemVisibility;

            public PriceItemViewModel(string supplier, string itemCode, double price) : base(supplier, itemCode, price)
            {
                IsEditing = false;
                ButtonText = "Edit";
                ButtonColor = Color.Parse("#512BD4");
                InputPrice = price.ToString();
                PriceItemVisibility = true;
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

            public string InputPrice
            {
                get => _inputPrice;
                set
                {
                    _inputPrice = value;
                    OnPropertyChanged(nameof(InputPrice));
                    ValidatePrice();
                }
            }

            public bool IsValidPrice
            {
                get => _isValidPrice;
                set
                {
                    _isValidPrice = value;
                    OnPropertyChanged(nameof(IsValidPrice));
                }
            }

            public bool PriceItemVisibility
            {
                get => _priceItemVisibility;
                set
                {
                    _priceItemVisibility = value;
                    OnPropertyChanged(nameof(PriceItemVisibility));
                }
            }

            public static List<PriceItemViewModel> ConvertToViewModels(IEnumerable<PriceItem> priceItems)
            {
                // Return the list of stock items as a list of stock item view models
                return priceItems.Select(item => new PriceItemViewModel(item.Supplier, item.ItemCode, item.Price)).ToList();
            }

            public void ValidatePrice()
            {
                IsValidPrice = double.TryParse(InputPrice, out double parsedPrice) && parsedPrice >= 0;
            }
        }
    }
}

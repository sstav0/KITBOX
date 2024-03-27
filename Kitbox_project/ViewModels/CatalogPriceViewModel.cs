
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
    internal class CatalogPriceViewModel : INotifyPropertyChanged
    {
        private List<CatalogPriceItemViewModel> _catalogPricesData;
        private DatabaseCatalogPrices DBCatalogPrices = new DatabaseCatalogPrices("kitboxer", "kitboxing");

        public CatalogPriceViewModel()
        {
            LoadDataAsync();
        }
        private async void LoadDataAsync()
        {
            var CatalogItems = await DBCatalogPrices.LoadAll();
            CatalogPricesData = CatalogPriceItemViewModel.ConvertToViewModels(DatabaseCatalogPrices.ConvertToPriceItem(CatalogItems));
        }

        public List<CatalogPriceItemViewModel> CatalogPricesData
        {
            get => _catalogPricesData;
            set
            {
                _catalogPricesData = value;
                OnPropertyChanged(nameof(CatalogPricesData));
            }
        }

        public void ApplyFilter(string searchText)
        {
            searchText = searchText.Trim();
            foreach (var item in CatalogPricesData)
            {
                // Filter items based on search text
                item.PriceItemVisibility =
                    string.IsNullOrWhiteSpace(searchText) ||
                    item.ItemCode.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            }
        }

        public async Task EditUpdatePrice(CatalogPriceItemViewModel priceItem)
        {
            // If Update button pressed
            if (priceItem.IsEditing)
            {
                // If the input price is a number and non-negative
                if (priceItem.IsValidPrice)
                {
                    priceItem.InputPrice = priceItem.InputPrice.TrimStart('0') != "" ? priceItem.InputPrice.TrimStart('0') : "0";

                    priceItem.Price = Convert.ToDouble(priceItem.InputPrice);
                    await DBCatalogPrices.Update(
                        new Dictionary<string, object> { { "Price", priceItem.Price } },
                        new Dictionary<string, object> { { "Code", priceItem.ItemCode } });

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

        public class CatalogPriceItemViewModel : CatalogPriceItem
        {
            private bool _isEditing;
            private string _buttonText;
            private Color _buttonColor;
            private string _inputPrice;
            private bool _isValidPrice;
            private bool _priceItemVisibility;

            public CatalogPriceItemViewModel(string itemCode, double price) : base(itemCode, price)
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

            public static List<CatalogPriceItemViewModel> ConvertToViewModels(IEnumerable<CatalogPriceItem> catalogPriceItems)
            {
                // Return the list of stock items as a list of stock item view models
                return catalogPriceItems.Select(item => new CatalogPriceItemViewModel(item.ItemCode, item.Price)).ToList();
            }

            public void ValidatePrice()
            {
                IsValidPrice = double.TryParse(InputPrice, out double parsedPrice) && parsedPrice >= 0;
            }
        }
    }
}

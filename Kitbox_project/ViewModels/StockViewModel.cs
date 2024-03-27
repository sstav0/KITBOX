using Kitbox_project.DataBase;
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
        private DatabaseStock DBStock = new DatabaseStock("kitboxer", "kitboxing");

        public StockViewModel()
        {
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var stockItems = await DBStock.LoadAll();
            StockData = StockItemViewModel.ConvertToViewModels(DatabaseStock.ConvertToStockItem(stockItems));
        }

        public List<StockItemViewModel> StockData
        {
            get => _stockData;
            set
            {
                _stockData = value;
                OnPropertyChanged(nameof(StockData));
            }
        }

        public void ApplyFilter(string searchText)
        {
            searchText = searchText.Trim();
            foreach (var item in StockData)
            {
                // Filter items based on search text
                item.StockItemVisibility =
                    string.IsNullOrWhiteSpace(searchText) ||
                    item.Reference.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    item.Code.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            }
        }

        public async Task EditUpdateQuantity(StockItemViewModel stockItem)
        {
            // If Update button pressed
            if (stockItem.IsEditing)
            {
                // If the input quantity is a number and non-negative
                if (stockItem.IsValidQuantity)
                {
                    stockItem.InputQuantity = stockItem.InputQuantity.TrimStart('0') != "" ? stockItem.InputQuantity.TrimStart('0') : "0";

                    stockItem.Quantity = Convert.ToInt32(stockItem.InputQuantity);
                    await DBStock.Update(
                        new Dictionary<string, object> { { "Quantity", stockItem.Quantity } },
                        new Dictionary<string, object> { { "idStock", stockItem.Id } });

                    stockItem.IsEditing = false;
                    stockItem.ButtonText = "Edit";
                    stockItem.ButtonColor = Color.Parse("#512BD4");
                }
                // If the input quantity is not a number or negative, keep the previous quantity
                else
                {
                    stockItem.InputQuantity = Convert.ToString(stockItem.Quantity);
                    stockItem.ButtonText = "Update";
                    stockItem.ButtonColor = Color.Parse("green");
                }
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

        // ViewModel for stock items
        public class StockItemViewModel : StockItem
        {
            private bool _isEditing;
            private string _buttonText;
            private Color _buttonColor;
            private string _inputQuantity;
            private bool _isValidQuantity;
            private bool _stockItemVisibility;
            private List<PriceItem> _priceItems;
            private double _catalogPrice;
            private bool _isInCatalog;
            private bool _isEditingPrice;
            private bool _isValidPrice;
            private string _InputPrice;

            private DatabaseSuppliers DBSupplierNames = new DatabaseSuppliers("kitboxer", "kitboxing");
            private DatabasePnD DBSupplierPrices = new DatabasePnD("kitboxer", "kitboxing");
            private DatabaseCatalogPrices DBCatalogPrices = new DatabaseCatalogPrices("kitboxer", "kitboxing");

            public StockItemViewModel(int id, string reference, string code, int quantity, int incomingQuantity, int outgoingQuantity) : base(id, reference, code, quantity, incomingQuantity, outgoingQuantity)
            {
                IsEditing = false;
                ButtonText = "Edit";
                ButtonColor = Color.Parse("#512BD4");
                InputQuantity = quantity.ToString();
                StockItemVisibility = true;
                IsInCatalog = true;

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

            public string InputQuantity
            {
                get => _inputQuantity;
                set
                {
                    _inputQuantity = value;
                    OnPropertyChanged(nameof(InputQuantity));
                    ValidateQuantity();
                }
            }

            public bool IsValidQuantity
            {
                get => _isValidQuantity;
                set
                {
                    _isValidQuantity = value;
                    OnPropertyChanged(nameof(IsValidQuantity));
                }
            }

            public bool StockItemVisibility
            {
                get => _stockItemVisibility;
                set
                {
                    _stockItemVisibility = value;
                    OnPropertyChanged(nameof(StockItemVisibility));
                }
            }

            public List<PriceItem> PriceItems
            {
                get => _priceItems;
                set
                {
                    _priceItems = value;
                    OnPropertyChanged(nameof(PriceItems));
                }
            }

            public double CatalogPrice
            {
                get => _catalogPrice;
                set
                {
                    _catalogPrice = value;
                    OnPropertyChanged(nameof(CatalogPrice));
                }
            }

            public bool IsInCatalog
            {
                get => _isInCatalog;
                set
                {
                    _isInCatalog = value;
                    OnPropertyChanged(nameof(IsInCatalog));
                }
            }

            public bool IsEditingPrice
            {
                get => _isEditingPrice;
                set
                {
                    _isEditingPrice = value;
                    OnPropertyChanged(nameof(IsEditingPrice));
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

            public string InputPrice
            {
                get => _InputPrice;
                set
                {
                    _InputPrice = value;
                    OnPropertyChanged(nameof(InputPrice));
                }
            }

            public static List<StockItemViewModel> ConvertToViewModels(IEnumerable<StockItem> stockItems)
            {
                // Return the list of stock items as a list of stock item view models
                return stockItems.Select(item => new StockItemViewModel(item.Id, item.Reference, item.Code, item.Quantity, item.IncomingQuantity, item.OutgoingQuantity)).ToList();
            }

            public void ValidateQuantity()
            {
                IsValidQuantity = int.TryParse(InputQuantity, out int parsedQuantity) && parsedQuantity >= 0;
            }

            public void ValidatePrice()
            {
                IsValidPrice = double.TryParse(InputPrice, out double parsedPrice) && parsedPrice >= 0;
            }


            public async void LoadPricesData()
            {
                var catalogDict = await DBSupplierPrices.GetData(
                    new Dictionary<string, string> { { "Code", Code } },
                    new List<string> { "Price", "idSupplier" });

                var i = 0;
                foreach (var catalogitem in catalogDict)
                {

                    var fakesupplierName = await DBSupplierNames.GetData(
                    new Dictionary<string, string> { { "idSuppliers", catalogitem["idSupplier"] } },
                    new List<string> { "NameofSuppliers" });

                    var supplierPrice = Convert.ToDouble(catalogDict[i]["Price"]);
                    var supplierName = fakesupplierName[0]["NameofSuppliers"];

                    PriceItems.Add(new PriceItem(supplierName, supplierPrice));

                    i++;
                }

                var catalogPrice = await DBCatalogPrices.GetData(
                    new Dictionary<string, string> { { "Code", Code } },
                    new List<string> { "Price" });

                CatalogPrice = Convert.ToDouble(catalogPrice[0]["Price"]);
            }
        }

        public class PriceItem
        {
            private string _supplierName;
            private double _supplierPrice;

            public PriceItem(string supplierName, double supplierPrice)
            {
                SupplierName = supplierName;
                SupplierPrice = supplierPrice;
            }

            public string SupplierName
            {
                get => _supplierName;
                set
                {
                    _supplierName = value;
                    OnPropertyChanged(nameof(SupplierName));
                }
            }

            public double SupplierPrice
            {
                get => _supplierPrice;
                set
                {
                    _supplierPrice = value;
                    OnPropertyChanged(nameof(SupplierPrice));
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }


}

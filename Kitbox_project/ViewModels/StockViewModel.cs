using Kitbox_project.DataBase;
using Kitbox_project.Models;
using Kitbox_project.Utilities;
using Kitbox_project.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kitbox_project.ViewModels
{
    public class StockViewModel : ILoginViewModel
    {
        private static List<StockItemViewModel> _stockData;
        private readonly DatabaseStock DBStock = new DatabaseStock("kitboxer", "kitboxing");
        private readonly DatabaseCatalog DBCatalog = new DatabaseCatalog("kitboxer", "kitboxing");

        public StockViewModel()
        {
            LoadDataAsync();

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(User))
                {
                    // Update visibilities 
                    UpdateUserRights(User);
                }
                if (e.PropertyName == nameof(IsDirector))
                {
                    UpdateDirectorRights();
                }
                if (e.PropertyName == nameof(IsSecretary))
                {
                    UpdateSecretaryRights();
                }
            };
        }

        private async void LoadDataAsync()
        {
            var stockItems = await DBStock.LoadAll();
            StockData = StockItemViewModel.ConvertToViewModels(DatabaseStock.ConvertToStockItem(stockItems));
            OnPropertyChanged(nameof(DisplayedStockData));

            User = Login.login;
        }

        public static List<StockItemViewModel> StockData
        {
            get => _stockData;
            set
            {
                _stockData = value;
            }
        }

        public List<StockItemViewModel> DisplayedStockData => StockData;

        public ICommand LogoutCommand => new Command(LogOutViewModel.LogoutButtonClicked);

        public static void UpdateStockQuantities(string code, int? quantity = null, int? incomingQuantity = null, int? outgoingQuantity = null)
        {
            var stockItem = StockData?.FirstOrDefault(item => item.Code == code);
            if (stockItem != null)
            {
                stockItem.Quantity = quantity.HasValue ? (int)quantity : stockItem.Quantity;
                stockItem.InputQuantity = Convert.ToString(stockItem.Quantity);
                stockItem.IncomingQuantity = incomingQuantity.HasValue ? (int)incomingQuantity : stockItem.IncomingQuantity;
                stockItem.OutgoingQuantity = outgoingQuantity.HasValue ? (int)outgoingQuantity : stockItem.OutgoingQuantity;
            }
        }

        private void UpdateDirectorRights()
        {
            foreach (var item in StockData)
            {
                item.IsDirector = IsDirector;
            }
        }

        private void UpdateSecretaryRights()
        {
            foreach (var item in StockData)
            {
                item.IsSecretary = IsSecretary;
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

        public async Task EditIsInCatalog(StockItemViewModel stockItem)
        {
            if(stockItem.InCatalog == true)
            {
                stockItem.InCatalog = false;
                stockItem.DirectorButtonText = "Add to Catalog";

                await DBCatalog.Update(
                        new Dictionary<string, object> { { "InCatalog", 0 } },
                        new Dictionary<string, object> { { "idStock", stockItem.Id } });
            }
            else
            {
                stockItem.InCatalog = true;
                stockItem.DirectorButtonText = "Remove from Catalog";

                await DBCatalog.Update(
                        new Dictionary<string, object> { { "InCatalog", 1 } },
                        new Dictionary<string, object> { { "idStock", stockItem.Id } });
            }
        }

        public async void AddInStock(StockItem stockItem, List<object> list)
        {
            Dictionary<string, object> dbStockItem = DatabaseStock.ConvertFromStockItem(stockItem);
            Dictionary<string, object> dbCatalogItem = DatabaseCatalog.ConvertFromStockItemAndStringList(stockItem, list);

            await DBStock.Add(dbStockItem);
            await DBCatalog.Add(dbCatalogItem);
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
            private string _directorButtonText;
            private bool _isDirector;
            private bool _isSecretary;

            private List<PriceItem> _priceItems = new List<PriceItem>();
            private double _catalogPrice;
            private bool _isEditingPrice;
            private string _priceButtonText;
            private Color _priceButtonColor;
            private bool _isValidPrice;
            private string _inputPrice;

            private readonly DatabaseSuppliers DBSupplierNames = new DatabaseSuppliers("kitboxer", "kitboxing");
            private readonly DatabasePnD DBSupplierPrices = new DatabasePnD("kitboxer", "kitboxing");
            private readonly DatabaseCatalog DBCatalog = new DatabaseCatalog("kitboxer", "kitboxing");
            private readonly DatabaseStock DBStock = new DatabaseStock("kitboxer", "kitboxing");

            public StockItemViewModel(int? id, string reference, string code, int quantity, int incomingQuantity, int outgoingQuantity, bool inCatalog) : base(id, reference, code, quantity, incomingQuantity, outgoingQuantity, inCatalog)
            {
                IsEditing = false;
                ButtonText = "Edit";
                ButtonColor = Color.Parse("#512BD4");
                InputQuantity = quantity.ToString();
                StockItemVisibility = true;

                IsEditingPrice = false;
                PriceButtonText = "Edit";
                PriceButtonColor = Color.Parse("#512BD4");
                DirectorButtonText = inCatalog ? "Remove from Catalog" : "Add to Catalog";

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

            public string DirectorButtonText
            {
                get => _directorButtonText;
                set
                {
                    _directorButtonText = value;
                    OnPropertyChanged(nameof(DirectorButtonText));
                }
            }

            public bool IsDirector
            {
                get => _isDirector;
                set
                {
                    _isDirector = value;
                    OnPropertyChanged(nameof(IsDirector));
                }
            }

            public bool IsSecretary
            {
                get => _isSecretary;
                set
                {
                    _isSecretary = value;
                    OnPropertyChanged(nameof(IsSecretary));
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

            public bool IsEditingPrice
            {
                get => _isEditingPrice;
                set
                {
                    _isEditingPrice = value;
                    OnPropertyChanged(nameof(IsEditingPrice));
                }
            }

            public string PriceButtonText
            {
                get => _priceButtonText;
                set
                {
                    _priceButtonText = value;
                    OnPropertyChanged(nameof(PriceButtonText));
                }
            }

            public Color PriceButtonColor
            {
                get => _priceButtonColor;
                set
                {
                    _priceButtonColor = value;
                    OnPropertyChanged(nameof(PriceButtonColor));
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
                get => _inputPrice;
                set
                {
                    _inputPrice = value;
                    OnPropertyChanged(nameof(InputPrice));
                    ValidatePrice();
                }
            }

            public static List<StockItemViewModel> ConvertToViewModels(IEnumerable<StockItem> stockItems)
            {
                // Return the list of stock items as a list of stock item view models
                return stockItems.Select(item => new StockItemViewModel(item.Id, item.Reference, item.Code, item.Quantity, item.IncomingQuantity, item.OutgoingQuantity, item.InCatalog)).ToList();
            }

            public async Task EditUpdateQuantity()
            {
                // If Update button pressed
                if (IsEditing)
                {
                    // If the input quantity is a number and non-negative
                    if (IsValidQuantity)
                    {
                        InputQuantity = InputQuantity.TrimStart('0') != "" ? InputQuantity.TrimStart('0') : "0";

                        Quantity = Convert.ToInt32(InputQuantity);
                        await DBStock.Update(
                            new Dictionary<string, object> { { "Quantity", Quantity.ToString() } },
                            new Dictionary<string, object> { { "idStock", Id.ToString() } });

                        IsEditing = false;
                        ButtonText = "Edit";
                        ButtonColor = Color.Parse("#512BD4");
                    }
                    // If the input quantity is not a number or negative, keep the previous quantity
                    else
                    {
                        InputQuantity = Convert.ToString(Quantity);
                        ButtonText = "Update";
                        ButtonColor = Color.Parse("green");
                    }
                }
                // If Edit button pressed
                else
                {
                    IsEditing = true;
                    ButtonText = "Update";
                    ButtonColor = Color.Parse("green");
                }
            }

            public async Task EditUpdatePrice()
            {
                // If Update button pressed
                if (IsEditingPrice)
                {
                    // If the input price is a number and non-negative
                    if (IsValidPrice)
                    {
                        InputPrice = InputPrice.Replace(".", ","); // Replace the dot with a comma just in case
                        CatalogPrice = Convert.ToDouble(InputPrice);
                        string CatalogPriceString = CatalogPrice.ToString();
                        CatalogPriceString = CatalogPriceString.Replace(",", "."); // Replace the comma with a dot otherwise DBCatalog.Update bugs

                        await DBCatalog.Update(
                            new Dictionary<string, object> { { "Price", CatalogPriceString } },
                            new Dictionary<string, object> { { "Code", Code } });

                        IsEditingPrice = false;
                        PriceButtonText = "Edit";
                        PriceButtonColor = Color.Parse("#512BD4");
                    }
                    // If the input price is not a number or negative, keep the previous price
                    else
                    {
                        InputPrice = Convert.ToString(CatalogPrice);
                        PriceButtonText = "Update";
                        PriceButtonColor = Color.Parse("green");
                    }
                }
                // If Edit button pressed
                else
                {
                    IsEditingPrice = true;
                    PriceButtonText = "Update";
                    PriceButtonColor = Color.Parse("green");
                }
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
                List<PriceItem> TempPriceItems = new List<PriceItem>();
                var catalogDict = await DBSupplierPrices.GetData(
                    new Dictionary<string, string> { { "Code", Code } },
                    new List<string> { "Price", "idSupplier" });

                foreach (var catalogitem in catalogDict)
                {
                    var fakesupplierName = await DBSupplierNames.GetData(
                        new Dictionary<string, string> { { "idSuppliers", catalogitem["idSupplier"] } },
                        new List<string> { "NameofSuppliers" });

                    var supplierPrice = Convert.ToDouble(catalogitem["Price"]);
                    var supplierName = fakesupplierName.FirstOrDefault()["NameofSuppliers"];

                    if (supplierName != null)
                    {
                        TempPriceItems.Add(new PriceItem(supplierName, supplierPrice));
                    }
                }

                var catalogPrice = await DBCatalog.GetData(
                    new Dictionary<string, string> { { "Code", Code } },
                    new List<string> { "Price" });

                CatalogPrice = catalogPrice.FirstOrDefault() != null ? Convert.ToDouble(catalogPrice[0]["Price"]) : 0.0;
                InputPrice = Convert.ToString(CatalogPrice);
                
                PriceItems = TempPriceItems;
                OnPropertyChanged(nameof(PriceItems));
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

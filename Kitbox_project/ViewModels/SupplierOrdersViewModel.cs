using Kitbox_project.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Kitbox_project.Views;
using System.Windows.Input;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Kitbox_project.DataBase;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kitbox_project.ViewModels
{
    internal class SupplierOrdersViewModel : INotifyPropertyChanged
    {
        private DatabaseSupplierOrders DBSupplierOrders = new DatabaseSupplierOrders("kitboxer", "kitboxing");
        private DatabaseSuppliers databaseSuppliers = new DatabaseSuppliers("kitboxer", "kitboxing");
        private DatabasePnD databasePnD = new DatabasePnD("kitboxer", "kitboxing");
        private DatabaseStock databaseStock = new DatabaseStock("kitboxer", "kitboxing");
        private DatabaseSupplierOrders databaseSupplierOrders = new("kitboxer", "kitboxing");
        private DatabaseSupplierOrderItem databaseSupplierOrderItem = new("kitboxer", "kitboxing");
        private List<SupplierOrderViewModel> _supplierOrders;
        private ObservableCollection<Supplier> _suppliers;
        private ObservableCollection<SupplierOrderItem> _tempOrderItems = new();
        private Supplier _selectedSupplier;
        private string _inputQuantity;
        private string _inputCode;
        private bool _isValidQuantity;
        private bool _isValidCode;
        private bool _isOrderNotEmpty;
        private double _tempOrderTotalPrice;

        public SupplierOrdersViewModel()
        {
            LoadDataAsync();
        }

        public SupplierOrdersViewModel(StockItem stockItem)
        {
            StockItem itemFromStockButton = stockItem; // Item to use to pre-fill the SupplierOrder form
            _inputCode = stockItem.Code;
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var supplierOrders = await DBSupplierOrders.LoadAll();
            var suppliers = await databaseSuppliers.LoadAll();
            if (supplierOrders is not null)
            {
                SupplierOrders = SupplierOrderViewModel.ConvertToViewModels(DatabaseSupplierOrders.ConvertToSupplierOrder(supplierOrders));
            }

            Suppliers = new ObservableCollection<Supplier>(DatabaseSuppliers.ConvertToSupplier(suppliers));
        }

        public List<SupplierOrderViewModel> SupplierOrders
        {
            get => _supplierOrders;
            set
            {
                _supplierOrders = value;
                OnPropertyChanged(nameof(SupplierOrders));
            }
        }

        public ObservableCollection<SupplierOrderItem> TempOrderItems
        {
            get => _tempOrderItems;
            set
            {
                _tempOrderItems = value;
                OnPropertyChanged(nameof(TempOrderItems));
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

        public string InputCode
        {
            get => _inputCode;
            set
            {
                _inputCode = value;
                OnPropertyChanged(nameof(InputCode));
                ValidateCode();
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

        public bool IsValidCode
        {
            get => _isValidCode;
            set
            {
                _isValidCode = value;
                OnPropertyChanged(nameof(IsValidCode));
            }
        }

        public bool IsOrderNotEmpty
        {
            get => _isOrderNotEmpty;
            set
            {
                _isOrderNotEmpty = value;
                OnPropertyChanged(nameof(IsOrderNotEmpty));
            }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get { return _suppliers; }
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }

        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
            }
        }

        public double TempOrderTotalPrice
        {
            get => _tempOrderTotalPrice;
            set
            {
                _tempOrderTotalPrice = value;
                OnPropertyChanged(nameof(TempOrderTotalPrice));
            }
        }

        public ICommand LogoutCommand => new Command(LogOutViewModel.LogoutButtonClicked);

        public void CheckSupplierSelection()
        {
            if (TempOrderItems.Count > 0)
            {
                IsOrderNotEmpty = true;
            }
            else
            {
                IsOrderNotEmpty = false;
            }
        }

        public void UpdateTempOrderTotalPrice()
        {
            double totalPrice = 0;
            foreach (var item in TempOrderItems)
            {
                totalPrice += item.TotalPrice;
            }
            TempOrderTotalPrice = Math.Round(totalPrice, 2);
        }

        public int GetLastOrderID()
        {
            int lastID = 0;
            if (SupplierOrders is not null)
            {
                foreach (var order in SupplierOrders)
                {
                    if (order.OrderID > lastID)
                    {
                        lastID = order.OrderID;
                    }
                }
            }
            return lastID + 1;
        }

        public async void AddNewItem(string itemCode, Object obj, int quantity)
        {
            SelectedSupplier = (Supplier)obj;

            var data = await databasePnD.GetData(
                               new Dictionary<string, string> { { "Code", itemCode }, { "idSupplier", SelectedSupplier.Id.ToString() } }
                               );

            if (data.Count == 1)
            {
                var reference = await databaseStock.GetData(new Dictionary<string, string> { { "Code", itemCode } }, new List<string> { "Reference" });

                TempOrderItems.Add(new SupplierOrderItem(
                    reference[0]["Reference"],
                    data[0]["Code"],
                    quantity,
                    double.Parse(data[0]["Price"])));
            }
            else if (data.Count >= 1)
            {
                throw new Exception("There are two items with the same code and the same supplier in the database");
            }

            UpdateTempOrderTotalPrice();
            CheckSupplierSelection();
        }

        public void ApplyFilter(string searchText)
        {
            searchText = searchText.Trim();
            foreach (var order in SupplierOrders)
            {
                // Filter orders based on search text
                order.SupplierOrderVisibility =
                    string.IsNullOrWhiteSpace(searchText) ||
                    order.OrderID.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    order.Item.Reference.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    order.Item.Code.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    order.SupplierName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    order.DeliveryDate.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            }
        }

        public void ApplyStatusFilter(bool isReceivedChecked, bool isOrderedChecked)
        {
            foreach (var order in SupplierOrders)
            {
                // Filter orders based on status
                order.SupplierOrderVisibility = (isReceivedChecked && order.Status == "Received") || (isOrderedChecked && order.Status == "Ordered");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void ValidateQuantity()
        {
            IsValidQuantity = int.TryParse(InputQuantity, out int parsedQuantity) && parsedQuantity >= 0;
        }

        public async void ValidateCode()
        {
            var res = await databaseStock.GetData(new Dictionary<string, string> { { "Code", InputCode } });

            if (res != null)
            {
                IsValidCode = true;
            }
            else
            {
                IsValidCode = false;
            }
        }

        public void DeleteItem(SupplierOrderItem item)
        {
            TempOrderItems.Remove(item);
            UpdateTempOrderTotalPrice();
            CheckSupplierSelection();
        }

        public async void AddNewSupplierOrder()
        {
            int orderId = GetLastOrderID();
            int orderWorstDelay = 0;

            if (TempOrderItems is not null)
            {
                foreach (var item in TempOrderItems)
                {
                    var itemPnDTable = await databasePnD.GetData(new Dictionary<string, string> { { "Code", item.Code }, { "idSupplier", SelectedSupplier.Id.ToString() } });

                    await databaseSupplierOrderItem.Add(new Dictionary<string, object>
                            {
                                {"idSupplierOrder", orderId.ToString() },
                                {"codeItem", item.Code },
                                {"quantity", item.Quantity.ToString() }
                            });


                    if (int.TryParse(itemPnDTable[0]["Delay"], out int itemDelay) && itemDelay > orderWorstDelay)
                    {
                        orderWorstDelay = itemDelay;
                    }
                }

                await databaseSupplierOrders.Add(new Dictionary<string, object>
                        {
                            {"idSupplier", SelectedSupplier.Id.ToString()},
                            {"deliveryDate", DateTime.Now.AddDays(orderWorstDelay).ToString("dd-MM-yyyy")},
                            {"price", TempOrderTotalPrice.ToString()},
                            {"status", "Ordered" }
                        });
            }

            TempOrderItems.Clear();
            UpdateTempOrderTotalPrice(); 
            CheckSupplierSelection();
            InputCode = "";
            InputQuantity = "";
        }
        public class SupplierOrderViewModel : SupplierOrder, INotifyPropertyChanged
        {
            private bool _supplierOrderVisibility;
            private bool _isExpanded;
            private string _supplierName;
            private readonly DatabaseSuppliers DBSuppliers = new DatabaseSuppliers("kitboxer", "kitboxing");
            private readonly DatabaseSupplierOrders DBSupplierOrder = new DatabaseSupplierOrders("kitboxer", "kitboxing");
            public ICommand OnReceivedClicked { get; }
            public ICommand OnCancelClicked { get; }

            public SupplierOrderViewModel(int orderID, int supplierId, string deliveryDate, double price, string status) : base(orderID, supplierId, deliveryDate, price, status)
            {
                _supplierOrderVisibility = true;
                _isExpanded = false;
                // DateTime.Now.AddDays(delay).ToString("dd/MM/yyyy");
                LoadSupplierName();
                OnReceivedClicked = new Command(ModifyOrderStatus);
                OnCancelClicked = new Command(CancelOrder);
            }

            //Launch a popup window to modify the status of the Supplier Order on the frontEnd and in the DB.dbo.SupplierOrder
            private async void ModifyOrderStatus()
            {
                bool orderReceived = await Application.Current.MainPage.DisplayAlert("Order Reception", "Confirm you received this order ?", "Yes", "No");
                Status = orderReceived ? "Received" : "Ordered";
                OnPropertyChanged(nameof(Status));
                UpdateDBOrderStatus();
            }

            //Launch a popup window to cancel the Supplier Order on the frontEnd and in the DB.dbo.SupplierOrder
            private async void CancelOrder()
            {
                bool orderCancelled = await Application.Current.MainPage.DisplayAlert("Order Cancellation", "Confirm you want to cancel this order ?", "Yes", "No");
                if (orderCancelled)
                {
                    //await DBSupplierOrder.Delete(new Dictionary<string, object> { { "idSupplierOrder", OrderID.ToString() } });
                }
            }

            public async void GetAllItems()
            {
                if (IsExpanded) return; // Don't load items if closing the expander

                //Step 1 => Get all items "codeItem", "quantity" where "idSupplierOrder" = OrderID
                DatabaseSupplierOrderItem databaseSupplierOrderItem = new DatabaseSupplierOrderItem("kitboxer", "kitboxing");
                var items = await databaseSupplierOrderItem.GetData(
                        new Dictionary<string, string> { { "idOrder", OrderID.ToString() } }, 
                        new List<string> {"codeItem", "quantity"});

                //Step 2 Get the infos to construct each SupplierOrderItem
                DatabasePnD databasePnD = new DatabasePnD("kitboxer", "kitboxing");
                DatabaseCatalog databaseCatalog = new DatabaseCatalog("kitboxer", "kitboxing");

                List<SupplierOrderItem> orderItems = new List<SupplierOrderItem>();
                foreach (var item in items)
                {
                    string code = item["codeItem"];
                    int quantity = int.Parse(item["quantity"]);

                    // Get "Price" from PnD where "Code" = codeItem (from step 1 below) and "idSupplier" = SupplierId (property from SupplierOrder class) 
                    var resPnD = await databasePnD.GetData(
                            new Dictionary<string, string> { { "Code", code }, { "idSupplier", SupplierId.ToString() } }, 
                            new List<string> { "Price" });

                    double unitPrice;
                    unitPrice = double.TryParse(resPnD[0]["Price"], out double result) ? result : throw new Exception("Price is not a number");

                    // Get "Reference" from Catalog where "Code" = codeItem (from step 1 below)
                    var resCatalog = await databaseCatalog.GetData(
                            new Dictionary<string, string> { { "Code", code } },
                            new List<string> { "Reference" });
                    string reference = resCatalog[0]["Reference"];

                    // Step 3 => Construct a new SupplierOrderItem with the infos from step 2 and add it to the list of SupplierOrderItems
                    orderItems.Add(new SupplierOrderItem(reference, code, quantity, unitPrice));
                }
                SupplierOrderItems = orderItems;
                OnPropertyChanged(nameof(SupplierOrderItems));
            }

            private async void LoadSupplierName()
            {
                var supplierName = await DBSuppliers.GetData(
                    new Dictionary<string, string> { { "idSuppliers", SupplierId.ToString() } }, 
                    new List<string> { "NameofSuppliers" });
                SupplierName = supplierName[0]["NameofSuppliers"];
            }

            //Update the status of an order in the DB.dbo.SupplierOrder
            private async void UpdateDBOrderStatus()
            {
                await DBSupplierOrder.Update(
                    new Dictionary<string, object> { { "status", Status } },
                    new Dictionary<string, object> { { "idSupplierOrder", OrderID.ToString()} });
            }

            public bool SupplierOrderVisibility
            {
                get => _supplierOrderVisibility;
                set
                {
                    _supplierOrderVisibility = value;
                    OnPropertyChanged(nameof(SupplierOrderVisibility));
                }
            }

            public bool IsExpanded
            {
                get => _isExpanded;
                set
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
                }
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

            public static List<SupplierOrderViewModel> ConvertToViewModels(IEnumerable<SupplierOrder> supplierOrders)
            {
                return supplierOrders.Select(order => new SupplierOrderViewModel(
                        order.OrderID,
                        order.SupplierId,
                        order.DeliveryDate,
                        order.Price,
                        order.Status
                    )).ToList();
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}

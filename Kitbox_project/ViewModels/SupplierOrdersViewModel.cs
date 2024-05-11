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
        private List<SupplierOrderViewModel> _supplierOrders;
        private DatabaseSupplierOrders DBSupplierOrders = new DatabaseSupplierOrders("kitboxer", "kitboxing");

        public SupplierOrdersViewModel()
        {
            LoadDataAsync();
        }

        public SupplierOrdersViewModel(StockItem stockItem)
        {
            StockItem itemFromStockButton = stockItem; // Item to use to pre-fill the SupplierOrder form
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var supplierOrders = await DBSupplierOrders.LoadAll();
            SupplierOrders = SupplierOrderViewModel.ConvertToViewModels(DatabaseSupplierOrders.ConvertToSupplierOrder(supplierOrders));
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

        public ICommand LogoutCommand => new Command(LogOutViewModel.LogoutButtonClicked);

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

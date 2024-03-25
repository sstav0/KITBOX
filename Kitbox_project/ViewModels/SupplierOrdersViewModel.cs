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

namespace Kitbox_project.ViewModels
{
    internal class SupplierOrdersViewModel : INotifyPropertyChanged
    {
        public ICommand OnReceivedClicked2 { get; }
        
        private List<SupplierOrderViewModel> _supplierOrders;
        private DatabaseSupplierOrders DBSupplierOrders = new DatabaseSupplierOrders("kitboxer", "kitboxing");

        public SupplierOrdersViewModel()
        {
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var supplierOrders = await DBSupplierOrders.LoadAll();
            SupplierOrders = SupplierOrderViewModel.ConvertToViewModels(await DatabaseSupplierOrders.ConvertToSupplierOrder(supplierOrders));
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
                    order.Date.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            }
        }

        public void ApplyStatusFilter(bool isChecked)
        {
            foreach (var order in SupplierOrders)
            {
                // Filter orders based on status
                order.SupplierOrderVisibility = !isChecked || order.Status == "Received";
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
            private List<SupplierOrderViewModel> _supplierOrders;
            private string _date;
            private string _supplierName;
            public ICommand OnReceivedClicked { get; }
            // private DatabaseSupplier DBSuppliers = new DatabaseSupplier("kitboxer", "kitboxing");
            private DatabaseSuppliers DBSuppliers = new DatabaseSuppliers("kitboxer", "kitboxing");
            private DatabaseSupplierOrders DBSupplierOrder = new DatabaseSupplierOrders("kitboxer", "kitboxing");

            public SupplierOrderViewModel(int orderID, StockItem item, int supplierId, int delay, int quantity, double price, string status) : base(orderID, item, supplierId, delay, quantity, price, status)
            {
                _supplierOrderVisibility = true;
                _date = DateTime.Now.AddDays(delay).ToString("dd/MM/yyyy");
                LoadSupplierName();
                OnReceivedClicked = new Command(ModifyOrderStatus);
            }

            //Launch a popup window to odify the status of the Supplier Order on the frontEnd and in the DB.dbo.SupplierOrder
            public async void ModifyOrderStatus()
            {
                bool orderReceived = await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Order Received ?", "Confirm you received this order ?", "Yes", "No");
                if(orderReceived)
                {
                    this.Status = "Received";
                    OnPropertyChanged(nameof(Status));
                }
                else
                {
                    this.Status = "Ordered";
                    OnPropertyChanged(nameof(Status));
                }
                UpdateDBOrderStatus();
                LoadSupplierName();
            }

            private async void LoadSupplierName()
            {
                var supplierName = await DBSuppliers.GetData(
                    new Dictionary<string, string> { { "idSuppliers", SupplierId.ToString() } }, 
                    new List<string> { "NameofSuppliers" }
                );
                SupplierName = supplierName[0]["NameofSuppliers"];
            }

            //Update the status of an order in the DB.dbo.SupplierOrder
            private async void UpdateDBOrderStatus()
            {
                Dictionary<string, object> newData = new Dictionary<string, object> { { "status", this.Status } };
                Dictionary<string, object> whereQuery = new Dictionary<string, object> { { "idSupplierOrder", this.OrderID} };
                await DBSupplierOrder.Update(newData, whereQuery);
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

            public string Date
            {
                get => _date;
                set
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
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
                        order.Item,
                        order.SupplierId,
                        order.Delay,
                        order.Quantity,
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

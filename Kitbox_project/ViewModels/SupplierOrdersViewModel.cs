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
            SupplierOrders = new List<SupplierOrderViewModel>
            {
                //new SupplierOrderViewModel(1, new StockItem(1, "Pannel", "PAN2144", 10), 1235, 12, 100, "Ordered"),
                //new SupplierOrderViewModel(1, new StockItem(1, "Pannel", "PAN2144", 10), 1245 , 12, 100, "Ordered")
            };
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

            public SupplierOrderViewModel(int orderID, StockItem item, int supplierId, int delay, int quantity, double price, string status) : base(orderID, item, supplierId, delay, quantity, price, status)
            {
                _supplierOrderVisibility = true;
                _date = DateTime.Now.AddDays(delay).ToString("dd/MM/yyyy");
                Debug.WriteLine(_date);
                // _supplierName = DBSuppliers.GetData(
                //     new Dictionary<string, string> { { "idSuppliers", supplierId.ToString() } }, new List<string> { "NameofSuppliers" }
                // );
                OnReceivedClicked = new Command(ModifyOrderStatus);
            }

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

using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        
        public SupplierOrdersViewModel()
        {
            SupplierOrders = new List<SupplierOrderViewModel>
            {
                new SupplierOrderViewModel(1, new StockItem(1, "Pannel", "PAN2144", 10), 1235, 12, 100, "Ordered"),
                new SupplierOrderViewModel(1, new StockItem(1, "Pannel", "PAN2144", 10), 1245 , 12, 100, "Ordered")
            };
            LoadDataAsync();
        }

        public async void LoadDataAsync()
        {
            // var supplierOrders = await DBStock.LoadAll();
            // SupplierOrders = SupplierOrderViewModel.ConvertToViewModels(DatabaseSupplier.ConvertToSupplierOrder(supplierOrders));
            //public static List<SupplierOrder> ConvertToSupplierOrder(List<Dictionary<string, string>> data)
            //{
            //    List<SupplierOrder> supplierOrders = new List<SupplierOrder>();
            //    foreach (var item in data)
            //    {
            //        supplierOrders.Add(new SupplierOrder(
            //            int.Parse(item["idStock"]),
            //            item["Reference"],
            //            item["Code"],
            //            int.Parse(item["Quantity"])
            //        ));
            //    }
            //    return supplierOrders;
            //}
        }
        private List<SupplierOrderViewModel> _supplierOrders;
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
            //searchText = searchText.Trim();
            //foreach (var item in StockData)
            //{
            //    // Filter items based on search text
            //    item.StockItemVisibility =
            //        string.IsNullOrWhiteSpace(searchText) ||
            //        item.Reference.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
            //        item.Code.Contains(searchText, StringComparison.OrdinalIgnoreCase);
            //}
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

            public SupplierOrderViewModel(int orderID, StockItem item, int supplierId, int delay, double price, string status) : base(orderID, item, supplierId, delay, price, status)
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

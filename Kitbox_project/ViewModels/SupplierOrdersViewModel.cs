using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Kitbox_project.ViewModels.StockViewModel;

namespace Kitbox_project.ViewModels
{
    internal class SupplierOrdersViewModel : INotifyPropertyChanged
    {
        private List<SupplierOrderViewModel> _supplierOrders;

        public SupplierOrdersViewModel()
        {
            SupplierOrders = new List<SupplierOrderViewModel>
            {
                new SupplierOrderViewModel(1, new StockItem(1, "Panel", "PAN2144", 10), 1, 7, 100, "Ordered")
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
            private string _date;
            private string _supplierName;
            // private DatabaseSupplier DBSuppliers = new DatabaseSupplier("kitboxer", "kitboxing");

            public SupplierOrderViewModel(int orderID, StockItem item, int supplierId, int delay, double price, string status) : base(orderID, item, supplierId, delay, price, status)
            {
                _supplierOrderVisibility = true;
                _date = DateTime.Now.AddDays(delay).ToString("dd/MM/yyyy");
                Debug.WriteLine(_date);
                // _supplierName = DBSuppliers.GetData(
                //     new Dictionary<string, string> { { "idSuppliers", supplierId.ToString() } }, new List<string> { "NameofSuppliers" }
                // );
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

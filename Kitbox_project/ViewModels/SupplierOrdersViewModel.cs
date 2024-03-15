using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    internal class SupplierOrdersViewModel : INotifyPropertyChanged
    {
        private List<SupplierOrderViewModel> _supplierOrders;

        public SupplierOrdersViewModel()
        {
            SupplierOrders = new List<SupplierOrderViewModel>
            {
                new SupplierOrderViewModel(1, new StockItem(1, "Pannel", "PAN2144", 10), "Supplier 1", DateTime.Today, 100, "Ordered")
            };
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

            public SupplierOrderViewModel(int orderID, StockItem item, string supplier, DateTime date, double price, string status) : base(orderID, item, supplier, date, price, status)
            {
                _supplierOrderVisibility = true;
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

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}

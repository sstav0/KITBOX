using Kitbox_project.DataBase;
using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace Kitbox_project;

public class SoldHistoryViewModel: INotifyPropertyChanged
{
    private List<OrderItemViewModel> _OrderData;
    private DatabaseOrder DBOrder = new DatabaseOrder("kitboxer", "kitboxing");
   
    public SoldHistoryViewModel()
        {
            LoadDataAsync();
        }

    

    private async Task LoadDataAsync()
    {
         var stockItems = await DBOrder.LoadAll();
        OrderData = OrderItemViewModel.ConvertToViewModels(DatabaseStock.ConvertToStockItem(OrderItems));
    }
    public List<OrderItemViewModel> OrderData
        {
            get => _OrderData;
            set
            {
                _OrderData = value;
                OnPropertyChanged(nameof(OrderData));
            }
        }

    public class OrderItemViewModel
    {
        
    }
    public static List<OrderItemViewModel> ConvertToViewModels(IEnumerable<StockItem> orderItems)
            {
                // Return the list of stock items as a list of stock item view models
                return orderItems.Select(item => new OrderItemViewModel(item.IdOrder, item.IdCustomer, item.Quantity,item.Code, item.Date)).ToList();
            }

      public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }        
}


using Kitbox_project.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models;

public class OrderItem : INotifyPropertyChanged
{
    private int _idOrder;
    private int _idCustomer;
    private Status.OrderStatus _status;
    private DateTime _creationTime;

    public OrderItem(int idOrder, int idCustomer, Status.OrderStatus status, DateTime creationTime)
    {
        _idOrder = idOrder;
        _idCustomer = idCustomer;
        _status = status;
        _creationTime = creationTime;
    }

    public int IdOrder
    {
        get => _idOrder;
    }

    public int IdCustomer
    {
        get => _idCustomer;
    }

    public Status.OrderStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(nameof(_status));
        }
    }

    public DateTime CreationTime
    {
        get => _creationTime;
    }

    public event PropertyChangedEventHandler PropertyChanged; 
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

public class OrderStockItem
{
    private StockItem _stockItem;
    private int _quantity;

    public OrderStockItem(StockItem stockItem, int quantity)
    {
        _stockItem = stockItem;
        _quantity = quantity;
    }

    public StockItem StockItem { get => _stockItem; set => _stockItem = value; }
    public int Quantity { get => _quantity; set => _quantity = value; }
}
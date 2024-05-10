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
    private Status.OrderStatus _orderStatus;
    private DateTime _creationTime;

    public OrderItem(int idOrder, int idCustomer, Status.OrderStatus orderStatus, DateTime creationTime)
    {
        _idOrder = idOrder;
        _idCustomer = idCustomer;
        _orderStatus = orderStatus;
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

    public Status.OrderStatus OrderStatus
    {
        get => _orderStatus;
        set
        {
            _orderStatus = value;
            OnPropertyChanged(nameof(OrderStatus));
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

public class OrderStockItem : StockItem
{
    private int _quantityInOrder;

    public OrderStockItem(int id, string reference, string code, int quantity, int incomingQuantity, int outgoingQuantity, bool inCatalog) : base(id, reference, code, quantity, incomingQuantity, outgoingQuantity, inCatalog)
    {
        _quantityInOrder = 0;
    }

    public int QuantityInOrder 
    { 
        get => _quantityInOrder;
        set
        {
            _quantityInOrder = value;
            OnPropertyChanged(nameof(QuantityInOrder));
        }
    }
}
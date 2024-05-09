using Kitbox_project.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kitbox_project.Utilities;
using static Kitbox_project.Utilities.Status;

namespace Kitbox_project.ViewModels;

public class OrderViewModel : INotifyPropertyChanged
{
    private List<OrderItemViewModel> _orders;
    private DatabaseOrder _dBOrders = new("kitboxer", "kitboxing");

    public OrderViewModel()
    {
        LoadAllOrders();
    }

    public List<OrderItemViewModel> Orders
    {
        get => _orders;
        set
        {
            _orders = value;
            OnPropertyChanged(nameof(_orders));
        }
    }

    private async void LoadAllOrders()
    {
        var orders = await _dBOrders.LoadAll();
        Orders = OrderItemViewModel.ConvertToViewModels(DatabaseOrder.ConvertToStockItem(orders));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class OrderItemViewModel : OrderItem
    {
        private List<OrderStockItem> _orderStockItems;
        private bool _orderItemVisibility;
        private string _notifaction;

        private DatabaseLocker _dBLockers = new("kitboxer", "kitboxing");
        private DatabaseCabinet _dBCabinets = new("kitboxer", "kitboxing");
        private DatabaseStock _dBStock = new("kitboxer", "kitboxing");
        
        private Dictionary<string, int> _refsAndQuantities = new();
        public OrderItemViewModel(int idOrder, int idCustomer, Status.OrderStatus status, DateTime creationTime) : base(idOrder, idCustomer, status, creationTime)
        {
            //LoadOrderStockItems();
            _orderItemVisibility = true;
            _notifaction = NotificationFromOrderStatus();
        }

        public List<OrderStockItem> OrderStockItems
        {
            get => _orderStockItems;
            set
            {
                _orderStockItems = value;
                OnPropertyChanged(nameof(_orderStockItems));
            }
        }

        public bool OrderItemVisibility
        {
            get => _orderItemVisibility;
            set
            {
                _orderItemVisibility = value;
                OnPropertyChanged(nameof(_orderItemVisibility));
            }
        }

        public string Notification
        {
            get => _notifaction;
            set
            {
                _notifaction = value;
                OnPropertyChanged(nameof(Notification));
            }
        }

        public string NotificationFromOrderStatus()
        {
            switch (Status)
            {
                case OrderStatus.WaitingConfirmation:
                    {
                        return "WaitingConfirmation";
                    }
                case OrderStatus.Ordered:
                    {
                        return "Ordered";
                    }
                case OrderStatus.WaitingPickup:
                    {
                        return "WaitingPickup";
                    }
                case OrderStatus.PickedUp:
                    {
                        return "PickedUp";
                    }
                case OrderStatus.Canceled:
                    {
                        return "Canceled";
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static List<OrderItemViewModel> ConvertToViewModels(List<OrderItem> orderItems)
        {
            return orderItems.Select(orderItem => new OrderItemViewModel(orderItem.IdOrder, orderItem.IdCustomer, orderItem.Status, orderItem.CreationTime)).ToList();
        }

        public async Task LoadOrderStockItems()
        {
            List<StockItem> stockItems = new();

            await GetRefsAndQuantity();
            List<int> quantities = _refsAndQuantities.Values.ToList();
            List<string> refs = _refsAndQuantities.Keys.ToList();
            Dictionary<string, string> conditionRefs = new();
            foreach(string reference in refs)
            {
                conditionRefs.Add("Code", reference);
            }
            var stockDict = await _dBStock.GetData(
                conditionRefs, new List<string> { "idStock", "Reference", "Code", "Quantity", 
                    "IncomingQuantity", "OutgoingQuantity", "InCatalog" });

            stockItems = DatabaseStock.ConvertToStockItem(stockDict);

            List<OrderStockItem> orderStockItems = new();

            int i = 0;
            foreach (var stockItem in stockItems)
            {
                orderStockItems.Add(new(stockItem, quantities[i]));
                i++;
            }
            OrderStockItems = orderStockItems;
        }

        private async Task GetRefsAndQuantity()
        {
            var CabinetDict = await _dBCabinets.GetData(
                new Dictionary<string, string> { { "idOrder", IdOrder.ToString() } },
                new List<string> { "idCabinet", "IronAngleRef" });
            // A list with each reference present in the lockers and cabinets
            List<string> refs = new();
            foreach (var cabinet in CabinetDict)
            {
                // Gets the value of the idCabinet, then uses it to get the other refs
                if (cabinet.TryGetValue("idCabinet", out string idCabinet))
                {
                    var LockerDict = await _dBLockers.GetData(
                    new Dictionary<string, string> { { "idCabinet", idCabinet } },
                    new List<string> { "sidePanelRef", "backPanelRef", "verticalBattenRef", "horizontalPanelRef",
                        "sideCrossbarRef", "frontCrossbarRef", "backCrossbarRef"});
                    foreach (var lockerRefs in LockerDict)
                    {
                        foreach (var lockerRef in lockerRefs.Values.ToList())
                        {
                            refs.Add(lockerRef);
                        }
                    }
                }
                if (cabinet.TryGetValue("IronAngleRef", out string ironAngleRef))
                {
                    refs.Add(ironAngleRef);
                }
            }
            _refsAndQuantities = refs.GroupBy(str => str).
                ToDictionary(group => group.Key, group => group.Count());
        }
    }

}

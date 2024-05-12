using Kitbox_project.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kitbox_project.Utilities;
using Kitbox_project.ViewModels;
using static Kitbox_project.Utilities.Status;
using System.Windows.Input;

namespace Kitbox_project.ViewModels;

public class OrderViewModel : INotifyPropertyChanged
{
    private List<OrderItemViewModel> _orders;
    private DatabaseOrder _dBOrders = new("kitboxer", "kitboxing");
    private DatabaseStock _dBStock = new("kitboxer", "kitboxing");

    private bool _activeOrdersVisible;
    private bool _unactiveOrdersVisible;

    public bool ActiveOrdersVisible
    {
        get => _activeOrdersVisible;
        set
        {
            _activeOrdersVisible = value;
            OnPropertyChanged(nameof(ActiveOrdersVisible));
        }
    }
    public ICommand LogoutCommand => new Command(LogOutViewModel.LogoutButtonClicked);
    public bool UnactiveOrdersVisible
    {
        get => _unactiveOrdersVisible;
        set
        {
            _unactiveOrdersVisible = value;
            OnPropertyChanged(nameof(UnactiveOrdersVisible));
        }
    }

    public OrderViewModel()
    {
        PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(ActiveOrdersVisible) || e.PropertyName == nameof(UnactiveOrdersVisible))
            {
                // Update visibilities 
                UpdateOrdersVisibilities();
            }
        };

        _activeOrdersVisible = true;
        _unactiveOrdersVisible = false;

        LoadAllOrders();
    }

    private void UpdateOrdersVisibilities()
    {
        foreach (OrderItemViewModel orderItemVM in Orders)
        {
            bool isActive = orderItemVM.OrderStatus != OrderStatus.Canceled && 
                orderItemVM.OrderStatus != OrderStatus.PickedUp;

            orderItemVM.OrderItemVisibility = (ActiveOrdersVisible && isActive) || 
                (UnactiveOrdersVisible && !isActive);
        }
    }

    public List<OrderItemViewModel> Orders
    {
        get => _orders;
        set
        {
            _orders = value;
            OnPropertyChanged(nameof(Orders));
        }
    }

    private async void LoadAllOrders()
    {
        var orders = await _dBOrders.LoadAll();
        Orders = OrderItemViewModel.ConvertToViewModels(DatabaseOrder.ConvertToOrderItem(orders));

        foreach (OrderItemViewModel orderItemVM in Orders)
        {
            await orderItemVM.LoadOrderStockItems();
        }
    }

    public async void ConfirmOrderStatus(OrderItemViewModel orderItemVM)
    {
        orderItemVM.ConfirmOrderStatus();

        await UpdateOrderStatus(orderItemVM);
    }

    public async void CancelOrder(OrderItemViewModel orderItemVM)
    {
        orderItemVM.CancelOrderStatus();

        await UpdateOrderStatus(orderItemVM);
    }

    private async Task UpdateOrderStatus(OrderItemViewModel orderItemVM)
    {
        string newOrderStatus = ConvertOrderStatusToString(orderItemVM.OrderStatus);

        await _dBOrders.Update(
            new Dictionary<string, object> { { "status", newOrderStatus } },
            new Dictionary<string, object> { { "idOrder", orderItemVM.IdOrder.ToString() } });
    }

    public void ApplyFilter(string searchText)
    {
        searchText = searchText.Trim();
        foreach (var item in Orders)
        {
            // Filter items based on search text
            item.OrderItemVisibility =
                string.IsNullOrWhiteSpace(searchText) ||
                item.IdOrder.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.IdCustomer.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }
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
        private string _stringedCreationTime;
        private string _stringedOrderStatus;
        private string _notifaction;
        private string _confirmButtonText;

        private DatabaseLocker _dBLockers = new("kitboxer", "kitboxing");
        private DatabaseCabinet _dBCabinets = new("kitboxer", "kitboxing");
        private DatabaseStock _dBStock = new("kitboxer", "kitboxing");

        public OrderItemViewModel(int idOrder, int idCustomer, OrderStatus orderStatus, DateTime creationTime) : base(idOrder, idCustomer, orderStatus, creationTime)
        {            
            if (OrderStatus is OrderStatus.Canceled || OrderStatus is OrderStatus.PickedUp)
            {
                _orderItemVisibility = false;
            }
            else
            {
                _orderItemVisibility = true;
            }

            _stringedCreationTime = creationTime.ToString();
            _stringedOrderStatus = Status.ConvertOrderStatusToString(OrderStatus);
            _confirmButtonText = ConfirmButtonTextFromOrderStatus();

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(OrderStatus))
                {
                    // Update properties dependent on OrderStatus

                    StringedOrderStatus = Status.ConvertOrderStatusToString(OrderStatus);
                    ConfirmButtonText = ConfirmButtonTextFromOrderStatus();
                }
                if (e.PropertyName == nameof(OrderStockItems))
                {
                    UpdateNotification();
                }
            };
        }

        public List<OrderStockItem> OrderStockItems
        {
            get => _orderStockItems;
            set
            {
                _orderStockItems = value;
                OnPropertyChanged(nameof(OrderStockItems));
            }
        }

        public string StringedOrderStatus
        {
            get => _stringedOrderStatus;
            set
            {
                _stringedOrderStatus = value;
                OnPropertyChanged(nameof(StringedOrderStatus));
            }
        }

        public string StringedCreationTime
        {
            get => _stringedCreationTime;
            set
            {
                _stringedCreationTime = value;
                OnPropertyChanged(nameof(StringedCreationTime));
            }
        }

        public bool OrderItemVisibility
        {
            get => _orderItemVisibility;
            set
            {
                _orderItemVisibility = value;
                OnPropertyChanged(nameof(OrderItemVisibility));
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

        public string ConfirmButtonText
        {
            get => _confirmButtonText;
            set
            {
                _confirmButtonText = value;
                OnPropertyChanged(nameof(ConfirmButtonText));
            }
        }

        private void UpdateNotification()
        {
            foreach(OrderStockItem orderStockItem in OrderStockItems)
            {
                if (orderStockItem.Quantity <= orderStockItem.QuantityInOrder)
                {
                    Notification = "Not enough items to fulfill the order";

                    break;
                }
                if (orderStockItem.Quantity <= orderStockItem.QuantityInOrder + 10)
                {
                    Notification = "Nearly not enough items to fulfill the order";

                    break;
                }
            }
        }
        private string ConfirmButtonTextFromOrderStatus()
        {
            switch (OrderStatus)
            {
                case OrderStatus.WaitingConfirmation:
                    {
                        return "Confirm Order";
                    }
                case OrderStatus.Ordered:
                    {
                        return "Confirm Readiness";
                    }
                case OrderStatus.WaitingPickup:
                    {
                        return "Confirm Pickup";
                    }
                case OrderStatus.PickedUp:
                    {
                        return "Close Order";
                    }
                case OrderStatus.Canceled:
                    {
                        return "Uncancel Order";
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public void ConfirmOrderStatus()
        {
            switch (OrderStatus)
            {
                case OrderStatus.WaitingConfirmation:
                    {
                        OrderStatus = OrderStatus.Ordered;
                        break;
                    }
                case OrderStatus.Ordered:
                    {
                        OrderStatus = OrderStatus.WaitingPickup;
                        break;
                    }
                case OrderStatus.WaitingPickup:
                    {
                        OrderStatus = OrderStatus.PickedUp;
                        break;
                    }
                case OrderStatus.PickedUp:
                    {
                        break;
                    }
                case OrderStatus.Canceled:
                    {
                        OrderStatus = OrderStatus.WaitingConfirmation;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public void CancelOrderStatus()
        {
            OrderStatus = OrderStatus.Canceled;
        }

        public static List<OrderItemViewModel> ConvertToViewModels(List<OrderItem> orderItems)
        {
            return orderItems.Select(orderItem => new OrderItemViewModel(orderItem.IdOrder, orderItem.IdCustomer, orderItem.OrderStatus, orderItem.CreationTime)).ToList();
        }

        public async Task LoadOrderStockItems()
        {
            List<OrderStockItem> stockItems = new();
            Dictionary<string, int> refsAndQuantities = await GetRefsAndQuantity();

            if(refsAndQuantities is not null)
            {

                List<int> quantities = refsAndQuantities.Values.ToList();
                List<string> refs = refsAndQuantities.Keys.ToList();

                List<Dictionary<string, string>> stockDictList = new();
                foreach (string reference in refs)
                {
                    var newStockDictList = await _dBStock.GetData(
                        new Dictionary<string, string> { { "Code", reference } },
                        new List<string> { "idStock", "Reference", "Code", "Quantity",
                    "IncomingQuantity", "OutgoingQuantity", "InCatalog" });

                    stockDictList = stockDictList.Concat(newStockDictList).ToList();
                }

                stockItems = DatabaseStock.ConvertToOrderStockItem(stockDictList);

                List<OrderStockItem> orderStockItems = AddQuantities(stockItems, quantities);

                OrderStockItems = orderStockItems;
            }
        }

        public async Task<Dictionary<string, int>> GetRefsAndQuantity()
        {
            Dictionary<string, int> refsAndQuantities = new();
            var CabinetDict = await _dBCabinets.GetData(
                new Dictionary<string, string> { { "idOrder", IdOrder.ToString() } },
                new List<string> { "idCabinet", "IronAngleRef" });
            // A list with each reference present in the lockers and cabinets
            List<string> refs = new();
            if(CabinetDict is not null)
            {
                foreach (var cabinet in CabinetDict)
                {
                    // Gets the value of the idCabinet, then uses it to get the other refs
                    if (cabinet.TryGetValue("idCabinet", out string idCabinet))
                    {
                        var lockerDict = await _dBLockers.GetData(
                        new Dictionary<string, string> { { "idCabinet", idCabinet } },
                        new List<string> { "sidePanelRef", "backPanelRef", "verticalBattenRef", "horizontalPanelRef",
                            "sideCrossbarRef", "frontCrossbarRef", "backCrossbarRef"});
                        if (lockerDict is not null) 
                        {
                            foreach (var lockerRefs in lockerDict)
                            {
                                foreach (var lockerRef in lockerRefs.Values.ToList())
                                {
                                    refs.Add(lockerRef);
                                }

                            }
                        }
                    }
                    if (cabinet.TryGetValue("IronAngleRef", out string ironAngleRef))
                    {
                        refs.Add(ironAngleRef);
                    }
                }
                 return refsAndQuantities = refs.GroupBy(str => str).
                    ToDictionary(group => group.Key, group => group.Count());
            }
            else
            {
                return refsAndQuantities;
            }
        }

        private List<OrderStockItem> AddQuantities(List<OrderStockItem> orderStockItems, List<int> quantities)
        {
            int i = 0;
            foreach (var orderStockItem in orderStockItems)
            {
                string firstLetters = orderStockItem.Code.Substring(0, 3);

                switch (firstLetters)
                {
                    case "COR":
                        // vertical battens = 4 per lvl
                        quantities[i] = quantities[i] * 4;
                        break;
                    case "PAH":
                        // horizontal panels = 2 per lvl
                        quantities[i] = quantities[i] * 2;
                        break;
                    case "PAG":
                        // side panels = 2 per lvl
                        quantities[i] = quantities[i] * 2;
                        break;
                    case "PAR":
                        // back panel = 1 per lvl
                        quantities[i] = quantities[i];
                        break;
                    case "POR":
                        // door panel = 1 per lvl (if present)
                        quantities[i] = quantities[i];
                        break;
                    case "TAS":
                        // vertical battens = 4 per cabinet
                        quantities[i] = quantities[i] * 4;
                        break;
                    case "TRF":
                        // front crossbars = 2 per lvl
                        quantities[i] = quantities[i] * 2;
                        break;
                    case "TRG":
                        // side crossbars = 4 per lvl
                        quantities[i] = quantities[i] * 4;
                        break;
                    case "TRR":
                        // back crossbars = 2 per lvl
                        quantities[i] = quantities[i] * 2;
                        break;
                    default:
                        break;
                }
                orderStockItem.QuantityInOrder = quantities[i];
                i++;
            }
            return orderStockItems;
        }
    }
}

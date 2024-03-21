using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Kitbox_project.Views;

public partial class OrdersPage : ContentPage
{
	private ObservableCollection<OrderViewModel> ListOrders;
    private ObservableCollection<OrderViewModel> ListShownOrders;
    private ObservableCollection<OrderViewModel> ListOrdersVoid;

    private List<Order> _orders;

    public List<Order> Orders
    {
        get => _orders;
        set
        {
            _orders = value;
        }
    }

    public OrdersPage()
    {
        InitializeComponent();

        this._orders = new List<Order>();
        this.Orders = new List<Order>();

        ListOrders = new ObservableCollection<OrderViewModel>();
        ListShownOrders = new ObservableCollection<OrderViewModel>();
        ListOrdersVoid = new ObservableCollection<OrderViewModel>();

        Debug.WriteLine(Orders.Count());

        FirstLoadRealOrders();

    }

    private void FirstLoadRealOrders()
    {
        foreach (Order order in Orders)
        {
            OrderViewModel newOrderViewModel = new OrderViewModel(order);

            ListOrders.Add(newOrderViewModel);
        }

        UpdateOrders();
    }

    private void LoadFalseOrdersClicked(object sender, EventArgs e)
    {
        Order order1 = new Order("Waiting Confirmation", new List<Cabinet>());
        Order order2 = new Order("Picked Up", new List<Cabinet>());

        OrderViewModel orderview1 = new OrderViewModel(order1);
        OrderViewModel orderview2 = new OrderViewModel(order2);

        orderview1.OrderVisibility = true;
        orderview2.OrderVisibility = false;

        ListOrders.Add(orderview1);
        ListOrders.Add(orderview2);

        UpdateOrders();
    }

    private void LoadRealOrders()
    {
        foreach (Order order in Orders)
        {
            OrderViewModel newOrderViewModel = new OrderViewModel(order);

            ListOrders.Add(newOrderViewModel);
        }

        UpdateOrders();
    }

    public void UpdateOrdersFromAfar(Order order)
    {
        OrderViewModel newOrderViewModel = new OrderViewModel(order);
        newOrderViewModel.OrderVisibility = true;
        ListOrders.Add(newOrderViewModel);

        UpdateOrders();
    }

    public void UpdateOrders()
    {
        ListShownOrders.Clear();
        foreach (var item in ListOrders)
        {
            if (item.OrderVisibility == true)
            {
                ListShownOrders.Add(item);
            }
        }

        ListViewOrders.ItemsSource = ListOrdersVoid;
        ListViewOrders.ItemsSource = ListShownOrders;
    }

    public void OnUpdateOrdersClicked(object sender, EventArgs e)
    {
        UpdateOrders();
    }


    private void CancelClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is OrderViewModel selectedOrderView)
        {
			selectedOrderView.Order.Status = "Canceled";
            selectedOrderView.OrderStatus = "Canceled";
        }
    }

	private void ReadyClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is OrderViewModel selectedOrderView)
        {
            if (selectedOrderView.OrderStatus == "Waiting Confirmation")
            {
                selectedOrderView.Order.Status = "Waiting Parts";
                selectedOrderView.OrderStatus = "Waiting Parts";

                //Orders.Find(selectedOrderView.OrderId).Status = "Waiting Parts";
            }

            if (selectedOrderView.OrderStatus == "Waiting Parts")
            {
                selectedOrderView.Order.Status = "Ready to PickUp";
                selectedOrderView.OrderStatus = "Ready to PickUp";

                //Orders.Find(selectedOrderView.OrderId).Status = "Ready to PickUp";
            }
        }
    }

    private void DisplayInStockButtonClicked(object sender, EventArgs e)
	{

	}

	private void PlaceOrderButtonClicked(object sender, EventArgs e)
	{

	}

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (sender is SearchBar searchBar && BindingContext is OrderViewModel orderViewModel)
        {
            //orderViewModel.ApplyFilter(searchBar.Text);
        }
    }

    public void ApplyFilter(string searchText)
    {
        //searchText = searchText.Trim();
        //foreach (OrderViewModel order in ListOrders)
        //{
        //    order.OrderVisibility =
        //        string.IsNullOrWhiteSpace(searchText) ||
        //        order.OrderID.Contains(searchText, StringComparison.OrdinalIgnoreCase)
        //}
    }

    private void OnActiveOrdersClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (button.TextColor == Colors.Black)
            {
                button.TextColor = Colors.White;
                button.BackgroundColor = Color.Parse("#512BD4");

                foreach(var item in ListOrders)
                {
                    if (item.OrderStatus == "Waiting Confirmation" ||
                        item.OrderStatus == "Waiting Parts" ||
                        item.OrderStatus == "Ready to PickUp")
                    {
                        item.OrderVisibility = false;
                    }
                }
            }

            else
            {
                button.TextColor = Colors.Black;
                button.BackgroundColor = Colors.Gray;

                foreach (var item in ListOrders)
                {
                    if (item.OrderStatus == "Waiting Confirmation" ||
                        item.OrderStatus == "Waiting Parts" ||
                        item.OrderStatus == "Ready to PickUp")
                    {
                        item.OrderVisibility = true;
                    }
                }
            }
        }

        UpdateOrders();
    }

    private void OnFinishedOrdersClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (button.TextColor == Colors.Black)
            {
                button.TextColor = Colors.White;
                button.BackgroundColor = Color.Parse("#512BD4");

                foreach (var item in ListOrders)
                {
                    if (item.OrderStatus == "Canceled" ||
                        item.OrderStatus == "Picked Up")
                    {
                        item.OrderVisibility = false;
                    }
                }
            }

            else
            {
                button.TextColor = Colors.Black;
                button.BackgroundColor = Colors.Gray;

                foreach (var item in ListOrders)
                {
                    if (item.OrderStatus == "Canceled" ||
                        item.OrderStatus == "Picked Up")
                    {
                        item.OrderVisibility = true;
                    }
                }
            }
        }

        UpdateOrders();
    }
}
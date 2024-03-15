using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Kitbox_project.Views;

public partial class OrdersPage : ContentPage
{
	private ObservableCollection<OrderViewModel> ListOrders;
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

    private Random rnd;

    public OrdersPage()
    {
        InitializeComponent();

        this._orders = new List<Order>();
        this.Orders = new List<Order>();

        ListOrders = new ObservableCollection<OrderViewModel>();
        ListOrdersVoid = new ObservableCollection<OrderViewModel>();

        rnd = new Random();


        Debug.WriteLine(Orders.Count());

        FirstLoadRealOrders();

    }

    private void FirstLoadRealOrders()
    {
        foreach (Order order in Orders)
        {
            int RandomID = rnd.Next(1, 101);

            //order.OrderID = RandomID;

            OrderViewModel newOrderViewModel = new OrderViewModel(order);

            ListOrders.Add(newOrderViewModel);
        }

        ListViewOrders.ItemsSource = ListOrders;
    }

    private void LoadRealOrders()
    {
        foreach (Order order in Orders)
        {
            OrderViewModel newOrderViewModel = new OrderViewModel(order);

            ListOrders.Add(newOrderViewModel);
        }

        ListViewOrders.ItemsSource = ListOrders;
    }

    private void LoadOrders()
	{
		Order order1 = new Order("Waiting Confirmation", new List<Cabinet>());

		Random rnd = new Random();
		//order1.OrderID = rnd.Next(1, 101); // Gives a random int between 1 & 100

		OrderViewModel orderviewmodel1 = new OrderViewModel(order1);

		Order order2 = new Order("Waiting Confirmation", new List<Cabinet>());

		//order2.OrderID = rnd.Next(1, 101);

		OrderViewModel orderviewmodel2 = new OrderViewModel(order2);
		orderviewmodel2.Notification = "Missing parts";

		ListOrders.Add(orderviewmodel1);
		ListOrders.Add(orderviewmodel2);

		ListViewOrders.ItemsSource = ListOrders;
    }

    public void UpdateOrdersFromAfar(Order order)
    {
        OrderViewModel newOrderViewModel = new OrderViewModel(order);
        ListOrders.Add(newOrderViewModel);

        ListViewOrders.ItemsSource = ListOrdersVoid;
        ListViewOrders.ItemsSource = ListOrders;
    }

    public void UpdateOrders()
    {
        ListViewOrders.ItemsSource = ListOrdersVoid;
        ListViewOrders.ItemsSource = ListOrders;
    }

    public void UpdateOrdersClicked(object sender, EventArgs e)
    {
        ListViewOrders.ItemsSource = ListOrdersVoid;
        ListViewOrders.ItemsSource = ListOrders;
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
            selectedOrderView.Order.Status = "Ready";
            selectedOrderView.OrderStatus = "Ready";
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
            orderViewModel.ApplyFilter(searchBar.Text);
        }
    }

    private void OnActiveOrdersClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (button.TextColor == Colors.Black)
            {
                button.TextColor = Colors.White;
                button.BackgroundColor = Color.Parse("#512BD4");
            }

            else
            {
                button.TextColor = Colors.Black;
                button.BackgroundColor = Colors.Gray;
            }
        }
    }

    private void OnFinishedOrdersClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (button.TextColor == Colors.Black)
            {
                button.TextColor = Colors.White;
                button.BackgroundColor = Color.Parse("#512BD4");
            }

            else
            {
                button.TextColor = Colors.Black;
                button.BackgroundColor = Colors.Gray;
            }
        }
    }
}
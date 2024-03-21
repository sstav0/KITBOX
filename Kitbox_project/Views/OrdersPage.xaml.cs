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

    private bool ActiveOrdersShown;
    private bool FinishedOrdersShown;

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

        ActiveOrdersShown = true;
        FinishedOrdersShown = false;

        FirstLoadRealOrders();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateOrders();
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
        foreach (var order in ListOrders)
        {
            if (ActiveOrdersShown == true)
            {
                if (order.OrderStatus == "Waiting Confirmation" ||
                   order.OrderStatus == "Waiting Parts" ||
                   order.OrderStatus == "Ready to PickUp")
                {
                    order.OrderVisibility = true;
                }
            }

            if (FinishedOrdersShown == true) 
            {
                if (order.OrderStatus == "Canceled" ||
                    order.OrderStatus == "Picked Up")
                {
                    order.OrderVisibility = true;
                }

            }

            if (ActiveOrdersShown == false)
            {
                if (order.OrderStatus == "Waiting Confirmation" ||
                   order.OrderStatus == "Waiting Parts" ||
                   order.OrderStatus == "Ready to PickUp")
                {
                    order.OrderVisibility = false;
                }
            }

            if (FinishedOrdersShown == false)
            {
                if (order.OrderStatus == "Canceled" ||
                    order.OrderStatus == "Picked Up")
                {
                    order.OrderVisibility = false;
                }
            } 
        }

        foreach(var order in ListOrders)
        {
            if (order.OrderVisibility == true)
            {
                ListShownOrders.Add(order);
            }
        }

        ListViewOrders.ItemsSource = ListOrdersVoid;
        ListViewOrders.ItemsSource = ListShownOrders;
    }

    public void OnUpdateOrdersClicked(object sender, EventArgs e)
    {
        UpdateOrders();
    }

    private async void AnyChangeStateButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is OrderViewModel selectedOrderView)
        {
            if(button.BackgroundColor == Colors.Red)
            {
                string mode = "Cancel";

                bool confirmation = await DisplayEnsurePopup(mode);

                CancelClicked(confirmation, selectedOrderView);
            }

            if (button.BackgroundColor == Colors.Green)
            {
                string mode = "Ready";

                bool confirmation = await DisplayEnsurePopup(mode);

                ReadyClicked(confirmation, selectedOrderView);
            }
        }
    }

    async Task<bool> DisplayEnsurePopup(string mode)
    {
        if (mode == "Cancel")
        {
            bool answer = await DisplayAlert("Confirmation required", "Do you want to cancel this order ?", "Yes", "No");
            return (answer);
        }

        if (mode == "Ready")
        {
            bool answer = await DisplayAlert("Confirmation required", "Do you want to set this order as ready ?", "Yes", "No");
            return (answer);
        }

        else
        {
            return false;
        }
    }

    private void CancelClicked(bool confirmation, OrderViewModel selectedOrderView)
    {
        if (confirmation == true)
        {
    		selectedOrderView.Order.Status = "Canceled";
            selectedOrderView.OrderStatus = "Canceled";

            UpdateOrders();
        }
    }

	private void ReadyClicked(bool confirmation, OrderViewModel selectedOrderView)
	{
        if (confirmation == true) 
        {
            if (selectedOrderView.OrderStatus == "Waiting Confirmation")
            {
                selectedOrderView.Order.Status = "Waiting Parts";
                selectedOrderView.OrderStatus = "Waiting Parts";

                //Orders.Find(selectedOrderView.OrderId).Status = "Waiting Parts";

                UpdateOrders();
            }

            if (selectedOrderView.OrderStatus == "Waiting Parts")
            {
                selectedOrderView.Order.Status = "Ready to PickUp";
                selectedOrderView.OrderStatus = "Ready to PickUp";

                //Orders.Find(selectedOrderView.OrderId).Status = "Ready to PickUp";

                UpdateOrders();
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

                ActiveOrdersShown = false;
            }

            else
            {
                button.TextColor = Colors.Black;
                button.BackgroundColor = Colors.Gray;

                ActiveOrdersShown = true;
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

                FinishedOrdersShown = false;
            }

            else
            {
                button.TextColor = Colors.Black;
                button.BackgroundColor = Colors.Gray;

                FinishedOrdersShown = true;
            }
        }

        UpdateOrders();
    }
}
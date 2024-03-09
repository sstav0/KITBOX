using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Kitbox_project.Views;

public partial class ActiveOrdersPage : ContentPage
{
	private ObservableCollection<OrderViewModel> ListOrders;

	public ActiveOrdersPage()
    {
        InitializeComponent();

        ListOrders = new ObservableCollection<OrderViewModel>();

        LoadOrders();

    }

	private void LoadOrders()
	{
		Order order1 = new Order("Waiting Confirmation", new List<Cabinet>());

		Random rnd = new Random();
		order1.OrderID = rnd.Next(1, 101); // Gives a random int between 1 & 100

		OrderViewModel orderviewmodel1 = new OrderViewModel(order1);

		Order order2 = new Order("Waiting Confirmation", new List<Cabinet>());

		order2.OrderID = rnd.Next(1, 101);

		OrderViewModel orderviewmodel2 = new OrderViewModel(order2);
		orderviewmodel2.Notification = "Missing parts";

		ListOrders.Add(orderviewmodel1);
		ListOrders.Add(orderviewmodel2);

		ListViewOrders.ItemsSource = ListOrders;
    }

	private void CancelClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is OrderViewModel selectedOrder)
        {
			selectedOrder.Order.Status = "Canceled";
        }
    }

	private void ReadyClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is OrderViewModel selectedOrder)
        {
            selectedOrder.Order.Status = "Ready";
        }
    }

	private void DisplayInStockButtonClicked(object sender, EventArgs e)
	{

	}

	private void PlaceOrderButtonClicked(object sender, EventArgs e)
	{

	}
}
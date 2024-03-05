using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Kitbox_project.Views;

public partial class CartPage : ContentPage
{
	private ObservableCollection<CartViewModel> Cart;
    private ObservableCollection<CartViewModel> CartVoid;
	private Order order;

    public CartPage(Order order)
	{
		InitializeComponent();
		Cart = new ObservableCollection<CartViewModel>();
		CartVoid = new ObservableCollection<CartViewModel>();

		LoadCart();
    }

	private void LoadCart()
	{
		if (order.Cart != null) { 
			string color1 = "red";
			Door door1 = new Door(color1, "wood", 50, 50); // 50x50 door (example)
			Door door1bis = new Door(color1, "wood", 50, 50);
			Locker locker1 = new Locker(50, 20, 50, color1, door1, 25);
			Locker locker1bis = new Locker(50, 20, 50, color1, door1bis, 25);
			List<Locker> lockers1 = new List<Locker>();
            lockers1.Add(locker1);
			lockers1.Add(locker1bis);
			Cabinet cabinet1 = new Cabinet(lockers1, 50, 75, 1);
			CartViewModel cabinet1view = new CartViewModel(cabinet1);

			Cart.Add(cabinet1view);

			ListCabinets.ItemsSource = Cart;

            UpdateTotalPrice();
        }

		else
		{
			foreach (Cabinet cabinet in order.Cart)
			{
                CartViewModel cabinetview = new CartViewModel(cabinet);

				Cart.Add(cabinetview);
            }

			ListCabinets.ItemsSource = Cart;

			UpdateTotalPrice();
		}
    }

	private void UpdateCart() 
	{
        ListCabinets.ItemsSource = CartVoid;
        ListCabinets.ItemsSource = Cart;

		UpdateTotalPrice();
	}

	private void UpdateTotalPrice()
	{
        double i = 0;
        foreach (CartViewModel item in Cart)
        {
            i += Convert.ToDouble(item.Price);
        }
        string totalPrice = $"{i.ToString()} €";
        TotalPrice.Text = totalPrice;
    }

	private async void OnAddNewClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CabinetCreatorPage());
    }

	private void OnConfirmClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is CartViewModel selectedCabinetView)
		{
            Debug.WriteLine("Confirm");
        }
    }

	private async void OnEditClicked(object sender, EventArgs e)
	{
		Debug.WriteLine("Editing");

		if(sender is Button button && button.CommandParameter is CartViewModel selectedCabinetView)
		{
			Cabinet selectedCabinet = selectedCabinetView.Cabinet;

			//await Navigation.PushAsync(new CabinetCreatorPage(selectedCabinet));
		}
	}

	private void OnDeleteClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is CartViewModel selectedCabinet)
        {
			Cart.Remove(selectedCabinet);
			UpdateCart();
        }
    }

	private void OnMoreClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is CartViewModel selectedCabinet)
        {
			selectedCabinet.Quantity += 1;
            UpdateCart();
        }
	}

	private void OnLessClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is CartViewModel selectedCabinet)
        {
            selectedCabinet.Quantity -= 1;
            UpdateCart();
        }
    }
}
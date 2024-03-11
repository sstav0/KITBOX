using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Kitbox_project.Views;

public partial class CartPage : ContentPage, INotifyPropertyChanged
{
	private ObservableCollection<CartViewModel> Cart;
    private ObservableCollection<CartViewModel> CartVoid;
	private Order order;

	//   public CartPage()
	//{
	//	InitializeComponent();
	//	Cart = new ObservableCollection<CartViewModel>();
	//	CartVoid = new ObservableCollection<CartViewModel>();

	//	order = new Order("InCreation", new List<Cabinet>());

	//	LoadCart();
	//   }

	public CartPage(Order order)
	{
		InitializeComponent();
		Cart = new ObservableCollection<CartViewModel>();
		CartVoid = new ObservableCollection<CartViewModel>();

		LoadRealCart(order);
	}

	private void LoadRealCart(Order order)
	{
		foreach (var item in order.Cart)
		{
			Cart.Add(new CartViewModel(item));
		}

        ListCabinets.ItemsSource = Cart;

        UpdateTotalPrice();
    }


    private void LoadCart()
	{
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
		CabinetCreatorPage newCabinetCreatorPage = new CabinetCreatorPage();
		newCabinetCreatorPage.Order = order;

        await Navigation.PushAsync(newCabinetCreatorPage);
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
		if(sender is Button button && button.CommandParameter is CartViewModel selectedCabinetView)
		{
			Cabinet selectedCabinet = selectedCabinetView.Cabinet;

            CabinetCreatorPage newCabinetCreatorPage = new CabinetCreatorPage();
            newCabinetCreatorPage.Order = order;
			newCabinetCreatorPage.Cabinet = selectedCabinet;
			newCabinetCreatorPage.IDCabinet = selectedCabinet.CabinetID;

            await Navigation.PushAsync(newCabinetCreatorPage);
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

    public event PropertyChangedEventHandler PropertyChanged; //utilité ?

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
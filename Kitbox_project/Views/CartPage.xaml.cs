using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using Kitbox_project.Views;
using Syncfusion.Maui.Core.Carousel;

namespace Kitbox_project.Views;

public partial class CartPage : ContentPage, INotifyPropertyChanged
{
	private ObservableCollection<CartViewModel> Cart;
    private ObservableCollection<CartViewModel> CartVoid;
	private Order order;
    private CabinetViewModel _cabviewModel;

    //public ICommand OnUpdateButtonClicked { get; }

    public CartPage(Order Order)
	{
		order = Order;

		InitializeComponent();
		Cart = new ObservableCollection<CartViewModel>();
		CartVoid = new ObservableCollection<CartViewModel>();
        LoadRealCart(order);

        LogOutButton.Command = new Command(() =>
        {
            var otherViewModel = new LogOutViewModel();
            otherViewModel.LogoutCommand.Execute(null);
        });

    }

    public void DisplayPopup()
    {
        PopupCustomerRec popup = new PopupCustomerRec(Cart);
        this.ShowPopup(popup);
    }

    private void LoadRealCart(Order order)
	{
		foreach (Cabinet cabinet in order.Cart)
		{
			Cart.Add(new CartViewModel(cabinet,this));
		}

        ListCabinets.ItemsSource = Cart;

        UpdateTotalPrice();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateCart();
    }

	public void UpdateCart() 
	{
        Cart.Clear();

        foreach (var cabinet in order.Cart)
        {
            CartViewModel cabinetview = new CartViewModel(cabinet, this);
            Cart.Add(cabinetview);
        }

        ListCabinets.ItemsSource = CartVoid;
        ListCabinets.ItemsSource = Cart;

		UpdateTotalPrice();
	}

	public void OnUpdateCartCLicked(object sender, EventArgs e)
	{
		UpdateCart();
	}

	private void UpdateTotalPrice()
	{
        double i = 0;
        foreach (Cabinet item in order.Cart)
        {
            i += Convert.ToDouble(item.Price);
        }
        string totalPrice = $"{i.ToString()} €";
        TotalPrice.Text = totalPrice;
    }

	private async void OnAddNewClicked(object sender, EventArgs e)
    {
		CabinetCreatorPage newCabinetCreatorPage = new CabinetCreatorPage(order);

        await Navigation.PushAsync(newCabinetCreatorPage);
    }

	private async void OnConfirmClicked(object sender, EventArgs e)
	{
		order.Status = "Waiting Confirmation";

        bool goToNextPage = false;
        DisplayPopup();

        if (goToNextPage)
        {
            OrdersPage newActiveOrdersPage = new OrdersPage();

            newActiveOrdersPage.Orders.Add(order);
            newActiveOrdersPage.UpdateOrdersFromAfar(order);

            await Navigation.PushAsync(newActiveOrdersPage);
        }


    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CartViewModel selectedCabinetView)
        {
            Cabinet selectedCabinet = selectedCabinetView.Cabinet;

            // Navigate to EditCabinetPage for editing with selected cabinet as parameter
            await Navigation.PushAsync(new EditCabinetPage(order, selectedCabinet));
        }
    }


    private void OnDeleteClicked(object sender, EventArgs e)
	{
        if (sender is Button button && button.CommandParameter is CartViewModel selectedCabinet)
        {
			order.Cart.RemoveAt(selectedCabinet.CabinetID);
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
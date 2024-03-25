using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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
	}

    //Display Popup window to ask if customer wants to confirm his order (return bool)
    async Task<bool> DisplayEnsureConfirmPopup()
    {
        bool answer = await DisplayAlert("Done ?", "Do you want to confirm your order ?", "Yes", "No");
        Debug.WriteLine("Answer: " + answer);
        return answer;
    }

    //Display popup window when invalid email input received
    async Task<bool> DisplayEnsureEmailPopup()
    {
        bool answer = await DisplayAlert("Invalid Input", "Do you want to retry ?", "Yes", "No");
        Debug.WriteLine("Answer: " + answer);
        return answer;
    }

    //display popup windows to ask for customer's email if email not valid -> call DisplayEnsureEmailPopup() else -> return true
    async Task<bool> DisplayEmailPopup()
    {
        string customerEmailFromPopup = await DisplayPromptAsync("Enter your email address", "Email", "OK", "Cancel", "@mail.com");
        bool goToNextPage = false;
        if (!string.IsNullOrWhiteSpace(customerEmailFromPopup) && customerEmailFromPopup.Any(char.IsLetterOrDigit) && customerEmailFromPopup.Contains('@'))
        {
            Debug.WriteLine("Customer email: " + customerEmailFromPopup);
            goToNextPage = true;

        }
        else
        {
            bool emailRetrial = await DisplayEnsureEmailPopup();
            if (emailRetrial)
            {
                goToNextPage = await DisplayEmailPopup();
            }
            else
            {
                customerEmailFromPopup = null;
                goToNextPage = false;
            }
            
        }
        return goToNextPage;
        
    }

    //Main Method that includes 3 others to display Popup windows and ask for customer's email
    async Task<bool> DisplayConfirmPopup()
    {
        bool confirmation = await DisplayEnsureConfirmPopup();
        if (confirmation)
        {
            bool goToNextPage = await DisplayEmailPopup();
            return goToNextPage;
        }
        else
        {
            return false;
        }
    }

    private void LoadRealCart(Order order)
	{
		foreach (Cabinet cabinet in order.Cart)
		{
			Cart.Add(new CartViewModel(cabinet));
		}

        ListCabinets.ItemsSource = Cart;

        UpdateTotalPrice();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateCart();
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
			Cabinet cabinet1 = new Cabinet(lockers1, 50, 75, 1, 3);
			CartViewModel cabinet1view = new CartViewModel(cabinet1);

			Cart.Add(cabinet1view);

			ListCabinets.ItemsSource = Cart;

            UpdateTotalPrice();
    }

	public void UpdateCart() 
	{
        Cart.Clear();

        foreach (var cabinet in order.Cart)
        {
            CartViewModel cabinetview = new CartViewModel(cabinet);
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
        bool goToNextPage = await DisplayConfirmPopup();
        
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
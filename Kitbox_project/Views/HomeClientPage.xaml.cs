using Kitbox_project.Models;

namespace Kitbox_project.Views;

public partial class HomeClientPage : ContentPage
{
	private Order newOrder;
	public HomeClientPage()
	{
		InitializeComponent();
	}

	private async void CreateNewOrderClicked(object sender, EventArgs e)
	{
		newOrder = new Order("In Creation", new List<Cabinet>());

		await Navigation.PushAsync(new CartPage(newOrder));
	}
}
using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;

namespace Kitbox_project.Views;

public partial class BasketPage : ContentPage
{
	private ObservableCollection<CabinetViewModelV2> Basket;
	public BasketPage()
	{
		InitializeComponent();
		Basket = new ObservableCollection<CabinetViewModelV2>();

		LoadBasket();
    }

	private void LoadBasket()
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
		CabinetViewModelV2 cabinet1view = new CabinetViewModelV2(cabinet1);

		Basket.Add(cabinet1view);

		cabinet1.GetHeight();
		ListCabinets.ItemsSource = Basket;
		double i = 0;
		foreach(CabinetViewModelV2 item in Basket)
		{
			i += Convert.ToDouble(item.GetPrice());
		}
		string totalPrice = $"{i.ToString()} €";
		TotalPrice.Text = totalPrice;
    }
}
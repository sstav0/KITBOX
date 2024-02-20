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
		DoorViewModel door1 = new DoorViewModel("gray", "wood");
        DoorViewModel door1bis = new DoorViewModel("brown", "wood");
        LockerViewModelV2 locker1 = new LockerViewModelV2(50, "brown", door1, "15");
        LockerViewModelV2 locker1bis = new LockerViewModelV2(40, "brown", door1bis, "25");
		ObservableCollection<LockerViewModelV2> list1 = new ObservableCollection<LockerViewModelV2>();
		list1.Add(locker1);
		list1.Add(locker1bis);
		CabinetViewModelV2 cabinet1 = new CabinetViewModelV2(list1, "40", "35", "60", "1");

		Basket.Add(cabinet1);

		cabinet1.GetHeight();
		ListCabinets.ItemsSource = Basket;
		TotalPrice.Text = "Soon";
	}
}
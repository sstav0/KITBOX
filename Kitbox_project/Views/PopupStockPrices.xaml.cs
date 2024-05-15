using CommunityToolkit.Maui.Views;
using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Diagnostics;

namespace Kitbox_project.Views;

public partial class PopupStockPrices : Popup
{
    private readonly StockViewModel.StockItemViewModel SelectedItem;
	public PopupStockPrices(StockViewModel.StockItemViewModel selectedItem)
	{
		InitializeComponent();
		BindingContext = selectedItem;
        SelectedItem = selectedItem;
        //LoadPricesDataAsync();
    }

    //private async void LoadPricesDataAsync()
    //{
    //    await SelectedItem.LoadPricesData();
    //}

    private async void OnEditPriceClicked(object sender, EventArgs e)
    {
        // Call the method EditUpdatePrice in the ViewModel
        await SelectedItem.EditUpdatePrice();
    }

    private async void DirectorButtonCLicked(object sender, EventArgs e)
    {
        // Call the method in the ViewModel
        await (BindingContext as StockViewModel).EditIsInCatalog(SelectedItem);
    }
}
using CommunityToolkit.Maui.Views;
using Kitbox_project.Models;
using Kitbox_project.ViewModels;

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

    private void OnEditPriceClicked(object sender, EventArgs e)
    {
        // Call the method in the ViewModel
        (BindingContext as StockViewModel)?.EditUpdatePrice(SelectedItem);
    }

    private void DirectorButtonCLicked(object sender, EventArgs e)
    {
        // Call the method in the ViewModel
        (BindingContext as StockViewModel)?.EditIsInCatalog(SelectedItem);
    }
}
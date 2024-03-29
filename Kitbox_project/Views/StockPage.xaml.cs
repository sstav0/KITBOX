using CommunityToolkit.Maui.Views;
using Kitbox_project.ViewModels;
using System.Diagnostics;

namespace Kitbox_project.Views;

public partial class StockPage : ContentPage
{
	public StockPage()
	{
		InitializeComponent();
	}

    private void OnEditUpdateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is StockViewModel.StockItemViewModel stockItem)
        {
            // Call the method in the ViewModel
            (BindingContext as StockViewModel)?.EditUpdateQuantity(stockItem);
        }
    }

    private void OpenPopupStockPrices(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is StockViewModel.StockItemViewModel selectedItem)
        {
            Debug.WriteLine("OpenPopupStockPrices");
            selectedItem.LoadPricesData();
            // Open popup with the stock prices
            var popup = new PopupStockPrices(selectedItem);
            this.ShowPopup(popup);
        }
    }

    private void OnOrderButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is StockViewModel.StockItemViewModel stockItem)
        {
            // Go to the Supplier Orders page with pre-fill data
        }
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (sender is SearchBar searchBar && BindingContext is StockViewModel stockViewModel)
        {
            stockViewModel.ApplyFilter(searchBar.Text);
        }
    }
}
using Kitbox_project.ViewModels;

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

    private void OpenPopupStockPrices(object sender, EventArgs e)
    {
        // Open popup with the stock prices
    }

    private void OnEditPriceClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is StockViewModel.StockItemViewModel stockItem)
        {
            // Call the method in the ViewModel
            (BindingContext as StockViewModel)?.EditUpdatePrice(stockItem);
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
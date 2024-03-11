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

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (sender is SearchBar searchBar && BindingContext is StockViewModel stockViewModel)
        {
            stockViewModel.ApplyFilter(searchBar.Text);
        }
    }

}
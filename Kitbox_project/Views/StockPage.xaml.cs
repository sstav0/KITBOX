using CommunityToolkit.Maui.Views;
using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Diagnostics;
using static Kitbox_project.Utilities.Users;

namespace Kitbox_project.Views;

public partial class StockPage : ContentPage
{
    private User _user;
    private bool isDirector;

    private string _newReference;
    private string _newCode;

	public StockPage()
	{
		InitializeComponent();

        _user = User.Director;

        if(_user is User.Director)
        {
            isDirector = true;
        }
	}

    private async void OnEditUpdateClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is StockViewModel.StockItemViewModel stockItem)
        {
            // Call the method in the ViewModel
            await stockItem.EditUpdateQuantity();
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

    private async void OnOrderButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is StockViewModel.StockItemViewModel stockItem)
        {
            // Go to the Supplier Orders page with pre-fill data
            var supplierOrdersPage = new SupplierOrdersPage(stockItem);
            await Navigation.PushAsync(supplierOrdersPage);
        }
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (sender is SearchBar searchBar && BindingContext is StockViewModel stockViewModel)
        {
            stockViewModel.ApplyFilter(searchBar.Text);
        }
    }

    private void OnAddStockItemButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && BindingContext is StockViewModel stockViewModel)
        {
            if (!string.IsNullOrEmpty(ReferenceEntry.Text) && !string.IsNullOrEmpty(CodeEntry.Text))
            {
                StockItem newStockItem = new(
                null, ReferenceEntry.Text, CodeEntry.Text, 0, 0, 0, false);

                List<object> list = new List<object>
                {
                    DimensionsEntry.Text, WidthEntry.Text, HeightEntry.Text,
                    CabinetHeightEntry.Text, DepthEntry.Text, 0,
                    ColorEntry.Text, PriceEntry.Text, MaterialEntry.Text
                };

                //foreach (string item in list)
                //{
                //    if (string.IsNullOrEmpty(item))
                //    {
                //        item = null;
                //    }
                //}

                stockViewModel.AddInStock(newStockItem, list);

                ResetEntries();
            }
        }
    }

    private void ResetEntries()
    {
        List<object> list = new List<object>
        {
            ReferenceEntry, CodeEntry, DimensionsEntry, WidthEntry, HeightEntry,
            CabinetHeightEntry, DepthEntry, ColorEntry, PriceEntry, MaterialEntry
        };

        foreach(Entry item in list)
        {
            item.Text = null;
        }
    }
}
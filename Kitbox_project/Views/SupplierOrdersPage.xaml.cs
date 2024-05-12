using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Views;
using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using Kitbox_project.Views;
using System.Diagnostics;
using System.Windows.Input;

namespace Kitbox_project.Views;

public partial class SupplierOrdersPage : ContentPage
{
    public SupplierOrdersPage()
    {
        InitializeComponent();
    }

    public SupplierOrdersPage(StockItem stockItem)
    {
        InitializeComponent();
        BindingContext = new SupplierOrdersViewModel(stockItem); // Creating the SupplierOrdersPage with the stock item from the StockPage 'Order' button
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (sender is SearchBar searchBar && BindingContext is SupplierOrdersViewModel supplierOrdersViewModel)
        {
            supplierOrdersViewModel.ApplyFilter(searchBar.Text);
        }
    }

    private async void OpenPopupNewSupplierOrder(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Done ?", "Do you want to confirm your order ?", "Yes", "No");
        if (answer)
        {
            if (sender is Button button && button.BindingContext is SupplierOrdersViewModel supplierOrdersViewModel)
            {
                supplierOrdersViewModel.AddNewSupplierOrder();
            }
        }
    }


    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && BindingContext is SupplierOrdersViewModel supplierOrdersViewModel)
        {
            // Should be based on all checkboxes values to filter accrodingly
            supplierOrdersViewModel.ApplyStatusFilter(receivedCheckBox.IsChecked, orderedCheckBox.IsChecked);
        }
    }

    private void OnExpanderClicked(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is SupplierOrdersViewModel.SupplierOrderViewModel supplierOrderViewModel)
        {
            supplierOrderViewModel.GetAllItems(); // Load all items for the supplier order
        }
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is SupplierOrdersViewModel supplierOrdersViewModel)
        {
            supplierOrdersViewModel.AddNewItem(ItemCode.Text, PickerSupplier.SelectedItem, int.Parse(Quantity.Text));
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is SupplierOrderItem supplierOrderItem)
        {
            ((SupplierOrdersViewModel)BindingContext).DeleteItem(supplierOrderItem);
        }
    }
}
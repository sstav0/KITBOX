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
        bool answer = await DisplayAlert("Order Confirmation", "Do you want to confirm your order ?", "Yes", "No");
        if (answer)
        {
            if (sender is Button button && button.BindingContext is SupplierOrdersViewModel supplierOrdersViewModel)
            {
                supplierOrdersViewModel.AddNewSupplierOrder();
                PickerSupplier.SelectedIndex = -1;
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

    private void OnCancelClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is SupplierOrdersViewModel.SupplierOrderViewModel supplierOrderViewModel)
        {
            (BindingContext as SupplierOrdersViewModel).CancelOrder(supplierOrderViewModel);
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
            if (PickerSupplier.SelectedIndex != -1)
            {
                supplierOrdersViewModel.AddNewItem(ItemCode.Text, (Supplier)PickerSupplier.SelectedItem, int.Parse(Quantity.Text));
            }
            else
            {
                DisplayAlert("Error", "Please select a supplier", "OK");
            }
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is SupplierOrderItem supplierOrderItem)
        {
            (BindingContext as SupplierOrdersViewModel).DeleteItem(supplierOrderItem);
        }
    }
}
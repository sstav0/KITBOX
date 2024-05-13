using CommunityToolkit.Maui.Views;
using Kitbox_project.ViewModels;

namespace Kitbox_project.Views;

public partial class OrdersPage : ContentPage
{
    public OrdersPage()
    {
        InitializeComponent();
    }

    private void OnActiveOrdersClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is OrderViewModel orderViewModel)
        {
            if (orderViewModel.ActiveOrdersVisible)
            {
                orderViewModel.ActiveOrdersVisible = false;

                ActiveOrdersButton.BackgroundColor = Colors.Gray;
                ActiveOrdersButton.TextColor = Colors.Black;
            }
            else
            {
                orderViewModel.ActiveOrdersVisible = true;

                ActiveOrdersButton.BackgroundColor = Color.FromRgba("#512BD4");
                ActiveOrdersButton.TextColor = Colors.White;
            }
        }
    }

    private void OnUnactiveOrdersClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is OrderViewModel orderViewModel)
        {
            if (orderViewModel.UnactiveOrdersVisible)
            {
                orderViewModel.UnactiveOrdersVisible = false;

                UnactiveOrdersButton.BackgroundColor = Colors.Gray;
                UnactiveOrdersButton.TextColor = Colors.Black;
            }
            else
            {
                orderViewModel.UnactiveOrdersVisible = true;

                UnactiveOrdersButton.BackgroundColor = Color.FromRgba("#512BD4");
                UnactiveOrdersButton.TextColor = Colors.White;
            }
        }
    }

    private void OpenPopupStockItems(object sender, EventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is OrderViewModel.OrderItemViewModel selectedOrderItemVM)
        {
            // Open popup with the stock prices
            var popup = new PopupOrderStockItems(selectedOrderItemVM);
            this.ShowPopup(popup);
        }
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
        if (sender is SearchBar searchBar && BindingContext is OrderViewModel orderViewModel)
        {
            orderViewModel.ApplyFilter(searchBar.Text);
        }
    }

    private void OnConfirmOrderButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is OrderViewModel.OrderItemViewModel orderItemVM)
        {
            // Call the method in the ViewModel
            (BindingContext as OrderViewModel)?.ConfirmOrderStatus(orderItemVM);
        }
    }
    private void OnCancelOrderButtonClicked(Object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is OrderViewModel.OrderItemViewModel orderItemVM)
        {
            if (orderItemVM.OrderStatus == Utilities.Status.OrderStatus.Canceled
                || orderItemVM.OrderStatus == Utilities.Status.OrderStatus.PickedUp)
            {
                (BindingContext as OrderViewModel)?.DeleteOrder(orderItemVM);
            }
            else
            {
                (BindingContext as OrderViewModel)?.CancelOrder(orderItemVM);
            }
        }
    }
}
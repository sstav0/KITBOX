using Kitbox_project.ViewModels;

namespace Kitbox_project.Views;

public partial class OrdersPage : ContentPage
{
    public OrdersPage()
    {
        InitializeComponent();
    }

    public void ApplyFilter(string searchText)
    {
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

    private void OnUpdateOrdersClicked(object sender, EventArgs e)
    {

    }

    private void LoadFalseOrdersClicked(object sender, EventArgs e)
    {

    }

    private void OnTextChanged(object sender, EventArgs e)
    {

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
            // Call the method in the ViewModel
            (BindingContext as OrderViewModel)?.CancelOrder(orderItemVM);
        }
    }
}
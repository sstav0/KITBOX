using CommunityToolkit.Maui.Views;
using Kitbox_project.ViewModels;

namespace Kitbox_project.Views;

public partial class PopupOrderStockItems : Popup
{
	private readonly OrderViewModel.OrderItemViewModel _selectedOrderItemVM;
	public PopupOrderStockItems(OrderViewModel.OrderItemViewModel selectedOrderItemVM)
    {
        InitializeComponent();

        BindingContext = selectedOrderItemVM;
		_selectedOrderItemVM = selectedOrderItemVM;

		LoadStockItems();
	}

    //private async void LoadStockItems()
    //{
    //	await (BindingContext as OrderViewModel)?.LoadOrderStockItems(_selectedOrderItemVM);
    //}

    private async void LoadStockItems()
    {
        await _selectedOrderItemVM.LoadOrderStockItems();
    }
}
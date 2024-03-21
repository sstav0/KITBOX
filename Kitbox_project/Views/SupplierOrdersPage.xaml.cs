using Kitbox_project.ViewModels;
using Kitbox_project.Views;
using System.Windows.Input;

namespace Kitbox_project.Views;

public partial class SupplierOrdersPage : ContentPage
{
    public SupplierOrdersPage()
	{
        InitializeComponent();
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
        bool answer = await  DisplayAlert("Done ?", "Do you want to confirm your order ?", "Yes", "No");
    }

}
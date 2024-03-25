using Kitbox_project.ViewModels;

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

    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && BindingContext is SupplierOrdersViewModel supplierOrdersViewModel)
        {
            if (checkBox == orderedCheckBox)
            {
                // Should be based on all checkboxes values to filter accrodingly
                supplierOrdersViewModel.ApplyStatusFilter(orderedCheckBox.IsChecked);
            }
        }
    }
}
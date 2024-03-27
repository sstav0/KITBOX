using Kitbox_project.ViewModels;

namespace Kitbox_project.Views;

public partial class SecretaryPricePage : ContentPage
{
    public SecretaryPricePage()
	{
		InitializeComponent();
    }

	private async void LoadPrices()
	{

	}

    private async void UpdatePrices()
    {

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdatePrices();
    }

    private void OnSavePriceClicked(object sender, EventArgs e)
	{

	}
}
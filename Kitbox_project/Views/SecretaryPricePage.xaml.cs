using Kitbox_project.ViewModels;

namespace Kitbox_project.Views;

public partial class SecretaryPricePage : ContentPage
{
	private List<PriceViewModel> Prices;
    private List<PriceViewModel> PricesVoid;

    public SecretaryPricePage()
	{
		InitializeComponent();

		Prices = new List<PriceViewModel>();
        PricesVoid = new List<PriceViewModel>();

        LoadPrices();
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
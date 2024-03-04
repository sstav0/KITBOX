namespace Kitbox_project.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private void TogglePasswordObscurationClicked(object sender, EventArgs e)
	{
		if(Password.IsPassword == true)
		{
			Password.IsPassword = false;
		}

		else
		{
			Password.IsPassword = true;
		}
	}
}
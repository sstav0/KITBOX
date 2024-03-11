namespace Kitbox_project.Views;
using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;


public partial class LoginPage : ContentPage
{
	private readonly DatabaseLogin _dbService;
	public LoginPage()
	{
		InitializeComponent();
		_dbService = new DatabaseLogin();
		BindingContext = this;
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
	private void LoginTryClicked(object sender, EventArgs e){
		string username = UserName.Text;
        string password = Password.Text;
		bool ValidLogin = _dbService.ValidateUser(username, password);
		if(ValidLogin)
		{
			if (username=="storekeeper"){
				Navigation.PushAsync(new StockPage());
			}
			else if(username=="customer"){
				Navigation.PushAsync(new HomeClientPage());
			}
		}

		else
            {
                // Display authentication failed message
                DisplayAlert("Login Failed", "Invalid username or password", "OK");
            }
	}
}
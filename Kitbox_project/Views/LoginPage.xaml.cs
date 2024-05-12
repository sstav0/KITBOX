namespace Kitbox_project.Views;
using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;
using Kitbox_project.LoginViews;


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
				Shell userShell = CreateShellForUser(username);
                Application.Current.MainPage = userShell;
		}

		else
            {
                // Display authentication failed message
                DisplayAlert("Login Failed", "Invalid username or password", "OK");
            }
	}
	 private Shell CreateShellForUser(string username)
     {
         // Dynamically create different shells based on the user's role
         switch (username)
         {
             case "customer":
                 return new CustomerLoginView();

			case "director":
				return new DirectorLoginView();
			case "storekeeper":
				return new StorekeeperLoginView();
			case "seller":
				return new SellerLoginView();
			case "secretary":
				return new SecretaryLoginView();	

             default:
                 // Handle other roles or default case
                 return new AppShell();
         }
     }
}
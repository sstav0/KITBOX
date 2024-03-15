namespace Kitbox_project.LoginViews;
using Kitbox_project.Views;

public partial class CustomerLoginView : Shell
{
	public CustomerLoginView()
	{
		InitializeComponent();
		 string login = Login.login = "customer";
         string password = Password.password = "customer";
	}
	
}
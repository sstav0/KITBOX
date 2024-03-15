namespace Kitbox_project.LoginViews;

public partial class CustomerLoginView : Shell
{
	public CustomerLoginView()
	{
		InitializeComponent();
		 string login = Login.login = "customer";
         string password = Password.password = "customer";
	}
}
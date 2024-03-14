namespace Kitbox_project.LoginViews;

public partial class CustomerLoginView : Shell
{
	public CustomerLoginView()
	{
		InitializeComponent();
		 string login = Login.login = "storekeeper";
         string password = Password.password = "storekeeper";
	}
}
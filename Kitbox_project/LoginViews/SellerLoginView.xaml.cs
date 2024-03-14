namespace Kitbox_project.LoginViews;

public partial class SellerLoginView : Shell
{
	public SellerLoginView()
	{
		InitializeComponent();
		string login = Login.login = "seller";
    	string password = Password.password = "seller";
	}
}
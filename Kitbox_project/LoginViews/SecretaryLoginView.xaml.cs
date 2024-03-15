namespace Kitbox_project.LoginViews;

public partial class SecretaryLoginView : Shell
{
	public SecretaryLoginView()
	{
		InitializeComponent();
		string login = Login.login = "secretary";
    	string password = Password.password = "secretary";
	}
}
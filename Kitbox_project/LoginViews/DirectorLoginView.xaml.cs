namespace Kitbox_project.LoginViews;

public partial class DirectorLoginView : Shell
{
	public DirectorLoginView()
	{
		InitializeComponent();
		string login = Login.login = "director";
    	string password = Password.password = "director";
	}
}
namespace Kitbox_project.LoginViews;

public partial class StorekeeperLoginView: Shell
{
	public StorekeeperLoginView()
	{
		InitializeComponent();
		string login = Login.login = "storekeeper";
         string password = Password.password = "storekeeper";
	}
}
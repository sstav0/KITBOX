namespace Kitbox_project;

public class Login
{
   private static string _login = "";

    public static string login
    {
        get { return _login; }
        set { _login = value; }
    }
}
public class Password
{
    private static string _password = "";

    public static string password
    {
        get { return _password; }
        set { _password = value; }
    }
}
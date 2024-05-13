namespace Kitbox_project;
using MySql.Data.MySqlClient;

public class DBService
{
    protected static  string ID =  Login.login ;
        
    protected  static string Psswrd =  Password.password;
    public static string connectionString
        {
            get
            {
                return $"Server=pat.infolab.ecam.be;port=63417;Database=KitBoxing;User ID={ID};Password={Psswrd};";
            }
        }
    private readonly MySqlConnection _connection;
    public DBService(){
        _connection = new MySqlConnection(connectionString);
       
    }
    public bool TestConnection()
    {
        try
        {
            _connection.Open();
            return true;
        }
        catch (MySqlException)
        {
            return false;
        }
        finally
        {
            _connection.Close();
        }
    }
  
    
}
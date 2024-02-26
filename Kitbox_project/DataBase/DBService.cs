namespace Kitbox_project;
using MySql.Data.MySqlClient;

public class DBService
{
    private const string connectionString = "Server= pat.infolab.ecam.be ; port=63417;Database=KitBoxing;User ID=kitboxer;Password=kitboxing;";
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
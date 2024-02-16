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
   public void AjouterElement(string firstname, string name, string email)
    {
        using (MySqlConnection connection = _connection)
        {
            connection.Open();

            // Remplacez "VotreTable", "Colonne1", "Colonne2", "Colonne3" par les noms réels de votre table et colonnes
            string query = "INSERT INTO Customer (firstname, user, email) VALUES (@firstname, @name, @email)";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);

                // Exécute la commande
                command.ExecuteNonQuery();
            }
        }
    }
    
}
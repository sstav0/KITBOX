namespace Kitbox_project;
using MySql.Data.MySqlClient;
public class DatabaseCustomer 
{
    private const string connectionString = "Server= pat.infolab.ecam.be ; port=63417;Database=KitBoxing;User ID=kitboxer;Password=kitboxing;";
    public void Add(string firstname, string name, string email)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString) )
        {
            connection.Open();

            // Remplacez "VotreTable", "Colonne1", "Colonne2", "Colonne3" par les noms réels de votre table et colonnes
            string query = "INSERT IGNORE INTO Customer (firstname, name, email) VALUES (@firstname, @name, @email)";
            
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
    public void Delete(int idCustomer)
    {
       using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Remplacez "VotreTable" et "VotreColonne" par les noms réels de votre table et colonne
            string query = "DELETE FROM Customer WHERE idCustomer = @idCustomer";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                // Exécute la commande
                command.ExecuteNonQuery();
            }
        
        }
    }
   
     public void Update(int idCustomer, string firstname, string name, string email)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Remplacez "VotreTable", "Colonne1", "Colonne2", "Colonne3" par les noms réels de votre table et colonnes
            string query = "UPDATE Customer SET firstname = @firstname, name = @name, email = @email WHERE idCustomer = @idCustomer";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                // Exécute la commande
                command.ExecuteNonQuery();
            }
        }
    }
    public string GetById(int idCustomer)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Remplacez "VotreTable", "VotreColonne" par les noms réels de votre table et colonne
            string query = "SELECT name FROM Customer WHERE idCustomer = @idCustomer";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["name"].ToString();
                    }
                }
            }
        }

        // Retournez une valeur par défaut si l'élément n'est pas trouvé
        return null;
    }
}

namespace Kitbox_project;

using System;
using MySql.Data.MySqlClient;
public class DatabaseCustomer 
{
    private const string connectionString = "Server= pat.infolab.ecam.be ; port=63417;Database=KitBoxing;User ID=kitboxer;Password=kitboxing;";
    public void Add(string firstname, string name, string email)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString) )
        {
            connection.Open();

            
            string query = "INSERT IGNORE INTO Customer (firstname, name, email) VALUES (@firstname, @name, @email)";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);

                
                command.ExecuteNonQuery();
            }
        }
    
    }
    public void Delete(int idCustomer)
    {
       using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "DELETE FROM Customer WHERE idCustomer = @idCustomer";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                command.ExecuteNonQuery();
            }
        
        }
    }
   
     public void Update(int idCustomer, string firstname, string name, string email)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "UPDATE Customer SET firstname = @firstname, name = @name, email = @email WHERE idCustomer = @idCustomer";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@firstname", firstname);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                
                command.ExecuteNonQuery();
            }
        }
    }
    public Dictionary<string, object> GetById(int idCustomer)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "SELECT * FROM Customer WHERE idCustomer = @idCustomer";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                         Dictionary<string, object> customerData = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object columnValue = reader.GetValue(i);
                            customerData.Add(columnName, columnValue);
                        }

                        return customerData;
                    }
                }
            }
        }

        // Retournez une valeur par défaut si l'élément n'est pas trouvé
        return null;
    }

    public static implicit operator DatabaseCustomer(DatabaseCatalog v)
    {
        throw new NotImplementedException();
    }
}

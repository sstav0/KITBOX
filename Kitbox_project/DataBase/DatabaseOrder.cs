namespace Kitbox_project;
using MySql.Data.MySqlClient;
public class DatabaseOrder
{
 private const string connectionString = "Server= pat.infolab.ecam.be ; port=63417;Database=KitBoxing;User ID=kitboxer;Password=kitboxing;";
    public void Add(string status, int idCustomer)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString) )
        {
            connection.Open();

            
            string query = "INSERT IGNORE INTO Order (status, idCustomer) VALUES (@status, @idCustomer)";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCustomer", idCustomer);
                command.Parameters.AddWithValue("@status", status);
               
                
                command.ExecuteNonQuery();
            }
        }
    
    }
    public void Delete(int idOrder)
    {
       using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "DELETE FROM Order WHERE idOrder = @idOrder";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idOrder", idOrder);

                command.ExecuteNonQuery();
            }
        
        }
    }
   
     public void Update(int idOrder, string status, int idCustomer)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "UPDATE Order SET status = @status, idCustomer = @idCustomer WHERE idOrder = @Order";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@idOrder", idOrder);
                command.Parameters.AddWithValue("@idCustomer", idCustomer);

                
                command.ExecuteNonQuery();
            }
        }
    }
    public Dictionary<string, object> GetById(int idOrder)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "SELECT * FROM Order WHERE idOrder = @idOrder";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idOrder", idOrder);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                         Dictionary<string, object> orderData = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object columnValue = reader.GetValue(i);
                            orderData.Add(columnName, columnValue);
                        }

                        return orderData;
                    }
                }
            }
        }

        // Retournez une valeur par défaut si l'élément n'est pas trouvé
        return null;
    }
}

namespace Kitbox_project;
using MySql.Data.MySqlClient;
public class DatabaseCabinet
{
    private const string connectionString = "Server= pat.infolab.ecam.be ; port=63417;Database=KitBoxing;User ID=kitboxer;Password=kitboxing;";
    public void Add(int price, int width, int height, int quantity)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString) )
        {
            connection.Open();

            
            string query = "INSERT IGNORE INTO Cabinet (price, width, height, quantity) VALUES (@price, @width, @height, @quantity)";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@width", width);
                command.Parameters.AddWithValue("@height", height);
                command.Parameters.AddWithValue("@quantity", quantity);

                // Exécute la commande
                command.ExecuteNonQuery();
            }
        }
    
    }
    public void Delete(int idCabinet)
    {
       using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

           
            string query = "DELETE FROM Cabinet WHERE idCabinet = @idCabinet";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCabinet", idCabinet);

                // Exécute la commande
                command.ExecuteNonQuery();
            }
        
        }
    }
   
     public void Update(int idCabinet, int price, int width, int height, int quantity)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "UPDATE Cabinet SET price = @price, width = @width, height = @height, quantity = @quantity WHERE idCabinet = @idCabinet";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@width", width);
                command.Parameters.AddWithValue("@height", height);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@idCabinet", idCabinet);

                
                command.ExecuteNonQuery();
            }
        }
    }
    public Dictionary<string, object> GetById(int idCabinet)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Cabinet WHERE idCabinet = @idCabinet";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCabinet", idCabinet);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Dictionary<string, object> cabinetData = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object columnValue = reader.GetValue(i);
                            cabinetData.Add(columnName, columnValue);
                        }

                        return cabinetData;
                    }
                }
            }
        }

        
        return null;
    }
}

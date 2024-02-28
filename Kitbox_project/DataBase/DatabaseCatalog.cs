namespace Kitbox_project;
using MySql.Data.MySqlClient;
public class DatabaseCatalog
{
    private const string connectionString = "Server= pat.infolab.ecam.be ; port=63417;Database=KitBoxing;User ID=kitboxer;Password=kitboxing;";
    /*public void Add(int Price , int Delay)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString) )
        {
            connection.Open();

            
            string query = "ALTER TABLE Catalog ADD COLUMN TO Order (status, idCustomer) VALUES (@status, @idCustomer)  WHERE Code =@Code";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCustomer", idCustomer);
                command.Parameters.AddWithValue("@status", status);
               
                
                command.ExecuteNonQuery();
            }
        }
    
    }
    public void AddSupplier(int Price , int Delay)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString) )
        {
            connection.Open();

            
            string query = "ALTER TABLE Catalog ADD COLUMN TO Order (status, idCustomer) VALUES (@status, @idCustomer)";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idCustomer", idCustomer);
                command.Parameters.AddWithValue("@status", status);
               
                
                command.ExecuteNonQuery();
            }
        }
    
    }*/
    public void Delete(string Code)
    {
       using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "DELETE FROM Catalog WHERE Code = @Code";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Code", Code);

                command.ExecuteNonQuery();
            }
        
        }
    }
   
     public void Update(string Column, string Update, string Code)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string query = "UPDATE Catalog SET @Column = @Update WHERE Code = @Code";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Column", Column);
                command.Parameters.AddWithValue("@Update", Update);
                command.Parameters.AddWithValue("@Code", Code);

                
                command.ExecuteNonQuery();
            }
        }
    }
    
        public List<Dictionary<string, object>> GetByReference(string Reference)
    {
        List<Dictionary<string, object>> catalogDataList = new List<Dictionary<string, object>>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM Catalog WHERE Reference = @Reference";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Reference", Reference);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> catalogData = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object columnValue = reader.GetValue(i);
                            catalogData.Add(columnName, columnValue);
                        }

                        catalogDataList.Add(catalogData);
                    }
                }
            }
        }

        // Return the list of dictionaries
        return catalogDataList;
    }

}


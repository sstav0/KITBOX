namespace Kitbox_project;

using System;
using Kitbox_project.DataBase;
using MySql.Data.MySqlClient;
public class DatabaseCustomer : Database
{
    public DatabaseCustomer(){
    tablename = "Customer";
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

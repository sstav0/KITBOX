using MySql.Data.MySqlClient;
namespace Kitbox_project.DataBase;
public class DatabaseLogin
{
    private readonly string connectionString = "Server= pat.infolab.ecam.be ; port=63417;Database=KitBoxing;User ID=login;Password=login;";
    public bool ValidateUser(string login, string password)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM Login WHERE Login = @login AND Password = @password";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", password);

                long count = (long)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}

using MySql.Data.MySqlClient;

namespace Kitbox_project;
public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly DatabaseCustomer _dbService;
        private readonly DatabaseCatalog _dbService2;
        public MainPage()
        {
            
            InitializeComponent();
            _dbService2 = new DatabaseCatalog();
            _dbService = new DatabaseCustomer();
            //TestDatabaseConnection();
        }
        /* private void TestDatabaseConnection()
    {
        bool isConnected = _dbService.TestConnection();

        if (isConnected)
        {
            Console.WriteLine("La connexion à la base de données a réussi.");
        }
        else
        {
            Console.WriteLine("Échec de la connexion à la base de données.");
        }
    }*/
       

        private void OnCounterClicked(object sender, EventArgs e)
        { 
            Dictionary<string, object> customerData = new Dictionary<string, object>
{
    { "firstname", "John" },
    { "name", "Doe" },
    { "email", "john.doe@example.com" }
};

            _dbService.Add(customerData);
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        private void OnCounterClicked2(object sender, EventArgs e)
        { 
           List<Dictionary<string, object>> dataList = _dbService2.GetByReference("Vertical batten");

            if (dataList != null && dataList.Count > 0)
            {
                Console.WriteLine($"Informations du client avec la référence 'Vertical batten':");

                foreach (var data in dataList)
                {
                    foreach (var kvp in data)
                    {
                        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }

                    Console.WriteLine(); // Add a newline between each row for better readability
                }
            }
            else
            {
                Console.WriteLine($"Aucun client trouvé avec la référence 'Vertical batten'");
            }
            count++;

            if (count == 1)
                CounterBtn2.Text = $"Clicked {count} time";
            else
                CounterBtn2.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn2.Text);
        }
    }
    



using MySql.Data.MySqlClient;

namespace Kitbox_project;
public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly DBService _dbService;
        public MainPage()
        {
            
            InitializeComponent();
            _dbService = new DBService();
            TestDatabaseConnection();
        }
         private void TestDatabaseConnection()
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
    }
       

        private void OnCounterClicked(object sender, EventArgs e)
        { 
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }



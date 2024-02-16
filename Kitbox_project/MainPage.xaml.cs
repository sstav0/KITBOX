using MySql.Data.MySqlClient;

namespace Kitbox_project;
public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly DatabaseCustomer _dbService;
        public MainPage()
        {
            
            InitializeComponent();
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
            _dbService.Add("Jean", "Dujardin", "coucou");
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        private void OnCounterClicked2(object sender, EventArgs e)
        { 
            string customerName = _dbService.GetById(14);

        // Affichage du nom du client dans la console
        if (customerName != null)
        {
            Console.WriteLine($"Le nom du client avec l'ID {14} est : {customerName}");
        }
        else
        {
            Console.WriteLine($"Aucun client trouvé avec l'ID {14}");
        }
            count++;

            if (count == 1)
                CounterBtn2.Text = $"Clicked {count} time";
            else
                CounterBtn2.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn2.Text);
        }
    }
    



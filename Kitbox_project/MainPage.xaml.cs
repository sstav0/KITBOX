using Kitbox_project.DataBase;
using Kitbox_project.Models;
using MySql.Data.MySqlClient;

namespace Kitbox_project;
public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly DatabaseCustomer _dbService;
        private readonly DatabaseCatalog _dbService2;
        private readonly DatabaseLogin _dbService3;
        
        string login = Login.login;
        string password = Password.password;
        public MainPage()
        {
            
            InitializeComponent();

        _dbService2 = new DatabaseCatalog(login, password);
            _dbService = new DatabaseCustomer(login, password);
            _dbService3 = new DatabaseLogin();
        }
       
       
    }
    



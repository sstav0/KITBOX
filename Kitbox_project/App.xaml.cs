using Kitbox_project.Views;

namespace Kitbox_project
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}

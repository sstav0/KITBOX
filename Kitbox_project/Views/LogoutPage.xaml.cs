using Microsoft.Maui.Controls;
using Kitbox_project;

namespace Kitbox_project.Views
{
    public partial class LogoutPage : ContentPage
    {
        public LogoutPage()
        {
            InitializeComponent();
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            
           Application.Current.MainPage = new AppShell();
        }
    }
}
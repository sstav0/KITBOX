using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using Kitbox_project.Views;

namespace Kitbox_project.ViewModels
{
    public class LogOutViewModel
    {
        public ICommand LogoutCommand { get; private set; }
        public LogOutViewModel()
        {
            LogoutCommand = new Command(LogoutButton_Clicked);
        }

        public async void LogoutButton_Clicked()
        {
            if (Microsoft.Maui.Controls.Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PopToRootAsync();
            }
            else if (Microsoft.Maui.Controls.Application.Current.MainPage is Shell shell)
            {
                await shell.GoToAsync($"//{nameof(HomeClientPage)}");
            }
        }
    }
}

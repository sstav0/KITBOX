using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using Kitbox_project.Views;
using Microsoft.Maui.Controls;

namespace Kitbox_project.ViewModels
{
    public class LogOutViewModel
    {
        public ICommand LogoutCommand { get; private set; }
        public LogOutViewModel()
        {
            LogoutCommand = new Command(LogoutButtonClicked);
        }

        public static void LogoutButtonClicked()
        {
            Microsoft.Maui.Controls.Application.Current.MainPage = new AppShell();
        }
    }
}

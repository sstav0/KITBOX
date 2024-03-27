using CommunityToolkit.Maui.Views;
using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Kitbox_project.Views
{
    public partial class PopupCustomerRec : Popup 
    {
        public PopupCustomerRec()
        {
            InitializeComponent();
            PopupCustomerRecViewModel viewModel = new PopupCustomerRecViewModel();
            this.BindingContext = viewModel;
        }   
    }
}


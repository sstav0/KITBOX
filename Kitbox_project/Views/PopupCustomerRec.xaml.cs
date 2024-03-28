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
    public partial class PopupCustomerRec : Popup, INotifyPropertyChanged
    {
        public PopupCustomerRec(ObservableCollection<CartViewModel> cart)
        {
            InitializeComponent();

            PopupCustomerRecViewModel _pupopCustomerViewModel = new PopupCustomerRecViewModel(cart);
            this.BindingContext = _pupopCustomerViewModel;
        }   
    }
}


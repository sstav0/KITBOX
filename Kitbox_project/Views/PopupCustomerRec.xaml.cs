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
        ContentPage _parentPage;
        public PopupCustomerRec(ObservableCollection<CartViewModel> cart, CartPage parentPage)
        {
            InitializeComponent();

            PopupCustomerRecViewModel viewModel = new PopupCustomerRecViewModel(cart, parentPage);
            this.BindingContext = viewModel;
            ContentPage _parentPage = parentPage;
        }
        public async void ClosePopup()
        {
            await _parentPage.Navigation.PopAsync();
        }
    }
}

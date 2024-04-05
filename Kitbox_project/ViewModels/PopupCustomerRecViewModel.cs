using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kitbox_project.Views;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Kitbox_project.ViewModels
{
    public class PopupCustomerRecViewModel
    {
        private ObservableCollection<CartViewModel> Cart;
        public event EventHandler PopupClosed;
        public Command OnOkButtonClicked { get; }
        DatabaseCustomer databaseCustomer = new DatabaseCustomer("customer", "customer");

        public PopupCustomerRecViewModel(ObservableCollection<CartViewModel> cart)
        {
            OnOkButtonClicked = new Command(RegisterEntry);
            Cart = cart;
            PopupText = "Complete Order";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _popupVisibility;
        public bool PopupVisibility
        {
            get { return _popupVisibility; }
            set
            {
                if (_popupVisibility != value)
                {
                    _popupVisibility = value;
                    OnPropertyChanged(nameof(PopupVisibility));
                }
            }
        }
        private string _popupText;
        public string PopupText
        {
            get { return _popupText; }
            set
            {
                if (_popupText != value)
                {
                    _popupText = value;
                    OnPropertyChanged(nameof(PopupText));
                }
            }
        }
        private string _entryFirstName;
        public string EntryFirstName
        {
            get { return _entryFirstName; }
            set
            {
                if (_entryFirstName != value)
                {
                    _entryFirstName = value;
                    OnPropertyChanged(nameof(EntryFirstName));
                }
            }
        }

        private string _entryLastName;
        public string EntryLastName
        {
            get { return _entryLastName; }
            set
            {
                if (_entryLastName != value)
                {
                    _entryLastName = value;
                    OnPropertyChanged(nameof(EntryLastName));
                }
            }
        }

        private string _entryEmail;
        public string EntryEmail
        {
            get { return _entryEmail; }
            set
            {
                if (_entryEmail != value)
                {
                    _entryEmail = value;
                    OnPropertyChanged(nameof(EntryEmail));
                }
            }
        }


        public bool IsDataInvalid()
        {
            bool invalidData = false;

            if (!string.IsNullOrWhiteSpace(_entryEmail) && _entryEmail.Any(char.IsLetterOrDigit) && _entryEmail.Contains('@'))
            {
                if (!string.IsNullOrWhiteSpace(_entryFirstName) && _entryFirstName.All(char.IsLetter))
                {
                    if (!string.IsNullOrWhiteSpace(_entryLastName) && _entryLastName.All(char.IsLetter))
                    {
                        invalidData = false;
                    }
                    else { invalidData = true; }
                }
                else { invalidData = true; }
            }
            else{ invalidData = true; }
            return invalidData;
        }

        public bool IsOrderEmpty()
        {
            if (Cart.Count() <= 0) { return true;}
            else { return false; }  
        }
        public async void RegisterEntry()
        {
            if (IsOrderEmpty() && IsDataInvalid())
            {
                Debug.WriteLine(PopupText);
                await Application.Current.MainPage.DisplayAlert("Invalid Input", "Retry ?","OK");
                await Shell.Current.Navigation.PopAsync();

                PopupCustomerRec newPopup = new PopupCustomerRec(Cart);
                Application.Current.MainPage.ShowPopup(newPopup);
            }
            else
            {
                Dictionary<string,object> dataCustomer = new Dictionary<string, object> { 
                    { "email", _entryEmail }, 
                    { "name", _entryLastName }, 
                    { "firstname", _entryFirstName } };
                await databaseCustomer.Add(dataCustomer);

            }
        }
    }
}

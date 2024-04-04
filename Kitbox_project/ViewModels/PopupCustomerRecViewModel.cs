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
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Kitbox_project.ViewModels
{
    public class PopupCustomerRecViewModel
    {
        private ObservableCollection<CartViewModel> Cart;
        public Command OnOkButtonClicked { get; }

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
            Debug.WriteLine("VerifyEntry()");
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

            Debug.WriteLine(invalidData.ToString());
            return invalidData;
        }

        public bool IsOrderEmpty()
        {
            if (Cart.Count() <= 0)
            {
                return true;
            }
            else { return false; }
            
        }
        private void ClosePopup()
        {
        }
        public void RegisterEntry()
        {
            if (IsOrderEmpty() && IsDataInvalid())
            {
                Debug.WriteLine(PopupText);
            }
        }
    }
}

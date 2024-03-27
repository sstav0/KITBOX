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

namespace Kitbox_project.ViewModels
{
    public class PopupCustomerRecViewModel
    {
        public Command OnOkButtonClicked { get; private set; }

        public PopupCustomerRecViewModel()
        {
            OnOkButtonClicked = new Command(VerifyEntry);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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


        public void VerifyEntry()
        {
            Debug.WriteLine("VerifyEntry()");
        }

    }
}

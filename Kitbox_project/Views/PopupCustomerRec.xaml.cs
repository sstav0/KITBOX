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

        Entry entry = new Entry { Placeholder = "Enter text" };

        public PopupCustomerRec()
        {
            InitializeComponent();
        }
        void OnOKButtonClicked(object? sender, EventArgs e) => Close();
        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;
            string myText = entry.Text;
        }

    }
}


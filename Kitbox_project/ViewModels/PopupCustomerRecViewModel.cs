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
using Newtonsoft.Json;

namespace Kitbox_project.ViewModels
{
    public class PopupCustomerRecViewModel
    {
        private ObservableCollection<CartViewModel> Cart;
        public event EventHandler PopupClosed;
        public Command OnOkButtonClicked { get; }
        DatabaseCustomer databaseCustomer = new DatabaseCustomer("customer", "customer");
        DatabaseOrder databaseOrder = new DatabaseOrder("customer", "customer");
        private CartPage _parentPage;

        public PopupCustomerRecViewModel(ObservableCollection<CartViewModel> cart, CartPage parentPage)
        {
            OnOkButtonClicked = new Command(RegisterEntry);
            Cart = cart;
            PopupText = "Complete Order";
            _parentPage = parentPage;
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
            }
            else
            {
                Dictionary<string,object> dataCustomer = new Dictionary<string, object> { 
                    { "email", _entryEmail }, 
                    { "name", _entryLastName }, 
                    { "firstname", _entryFirstName } };
                await databaseCustomer.Add(dataCustomer);

                //system to get Customer ID
                Dictionary<string, string> dataCustomerString = new Dictionary<string, string>();

                foreach (var kvp in dataCustomer)
                {
                    dataCustomerString.Add(kvp.Key, kvp.Value.ToString());
                }
                List<Dictionary<string, string>> customerDataList = await databaseCustomer.GetData(dataCustomerString);

                if (dataCustomerString.Count <= 0 || customerDataList == null) {
                    customerDataList = await databaseCustomer.GetData(dataCustomerString);
                    await Application.Current.MainPage.DisplayAlert("ERROR 1", "We encounter problems trying to get your order registered", "OK");
                }

                else
                {
                    string idCustomer = customerDataList[customerDataList.Count - 1]["idCustomer"].ToString();
                    //system to add the order in the DB
                    Dictionary<string, object> dataOrder = new Dictionary<string, object> {
                    { "idCustomer", Int32.Parse(idCustomer) },
                    { "status", "Ordered" },
                    { "DateTimeColumn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
                    };
                    await databaseOrder.Add(dataOrder);

                    // system to get Order ID
                    Dictionary<string, string> dataOrderString = new Dictionary<string, string>();
                    foreach (var kvp in dataOrder)
                    {
                        dataOrderString.Add(kvp.Key, kvp.Value.ToString());
                    }

                    List<Dictionary<string, string>> orderDataList = await databaseOrder.GetData(dataOrderString);
                    if (dataOrderString.Count <= 0 || orderDataList == null)
                    {
                        orderDataList = await databaseOrder.GetData(dataOrderString);
                        await Application.Current.MainPage.DisplayAlert("ERROR 2", "We encounter problems trying to get your order registered", "OK");
                    }
                    else
                    {
                        string idOrder = orderDataList[orderDataList.Count - 1]["idOrder"].ToString();
                        //Clsoe Popup
                        _parentPage.ClosePopup();
                        //Back to Main Page (Customer)
                        Microsoft.Maui.Controls.Application.Current.MainPage = new AppShell();
                        //Display message on a new popup (with ID order & customer)
                        string orderIdMessage = $"Validate your Order with one of our sellers \nCustomer ID : {idCustomer} \nOrder ID : {idOrder}";
                        await Application.Current.MainPage.DisplayAlert("Successfully Registered your Order", orderIdMessage, "OK");
                        
                    }
                }
            }
        }
    }
}

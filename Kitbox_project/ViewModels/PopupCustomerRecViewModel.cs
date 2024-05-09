using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kitbox_project.Views;
using Kitbox_project.Models;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using TEST_ORM;
using Kitbox_project.DataBase;

namespace Kitbox_project.ViewModels
{
    public class PopupCustomerRecViewModel
    {
        private ObservableCollection<CartViewModel> Cart;
        public event EventHandler PopupClosed;
        public Command OnOkButtonClicked { get; }

        DatabaseCustomer databaseCustomer = new DatabaseCustomer("customer", "customer");
        DatabaseOrder databaseOrder = new DatabaseOrder("customer", "customer");
        DatabaseCabinet databaseCabinet = new DatabaseCabinet("customer", "customer");
        DatabaseLocker databaseLocker = new DatabaseLocker("customer", "customer");
        DatabaseCatalog databaseCatalog = new DatabaseCatalog("storekeeper", "storekeeper");
        private CartPage _parentPage;
        private string idOrder;
        private string idCustomer;
        private string idCabinet;

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

        /// <summary>
        /// Checks if the entered personal data is invalid.
        /// </summary>
        /// <returns>True if the entered personal data is invalid; otherwise, false.</returns>
        /// <remarks>
        /// This method evaluates the validity of entered personal data, including email, first name, and last name. 
        /// It checks if the email contains alphanumeric characters and the '@' symbol, and if both first and last 
        /// names are not empty and consist of letters or white spaces. If any of these conditions are not met, the 
        /// method returns true, indicating invalid data; otherwise, it returns false.
        /// </remarks>
        public bool IsDataInvalid()
        {
            bool invalidData = false;

            if (_entryEmail.Any(char.IsLetterOrDigit)  && !string.IsNullOrWhiteSpace(_entryEmail) &&  _entryEmail.Contains('@'))
            {
                if (!string.IsNullOrWhiteSpace(_entryFirstName) && (_entryFirstName.All(char.IsLetter) || _entryLastName.Any(char.IsWhiteSpace)))
                {
                    if (!string.IsNullOrWhiteSpace(_entryLastName) && (_entryLastName.All(char.IsLetter) || _entryLastName.Any(char.IsWhiteSpace)))
                    {
                        invalidData = false;
                    }
                    else { invalidData = true; }
                }
                else { invalidData = true; }
            }
            else { invalidData = true; }
            return invalidData;
        }

        public bool IsOrderEmpty()
        {
            if (Cart.Count() <= 0) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Registers the customer's entry, including customer details and cart items.
        /// </summary>
        /// <remarks>
        /// This method initiates the registration process for the customer by calling the <see cref="RegisterCustomer"/> 
        /// method to register the customer's personal details and the <see cref="RegisterCart"/> method to register 
        /// the cart items. If the registration process completes successfully, it closes the current popup, navigates 
        /// back to the main page, and displays a success message containing the customer ID and order ID. If any errors 
        /// occur during the registration process, an error message is displayed.
        /// </remarks>
        public async void RegisterEntry()
        {
            bool goToCartStep =await RegisterCustomer();
            bool endRegistery =await RegisterCart(goToCartStep);
            if (endRegistery)
            {
                //Clsoe Popup
                _parentPage.ClosePopup();
                //Back to Main Page (Customer)
                Microsoft.Maui.Controls.Application.Current.MainPage = new AppShell();
                //Display message on a new popup (with ID order & customer)
                string orderIdMessage = $"Validate your Order with one of our sellers \nCustomer ID : {idCustomer} \nOrder ID : {idOrder}";
                await Application.Current.MainPage.DisplayAlert("Successfully Registered your Order", orderIdMessage, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("ERROR 4", "We encounter problems trying to get your order registered", "OK");
            }
        }
        /// <summary>
        /// Registers a customer and their order details in the database.
        /// </summary>
        /// <returns>
        /// A boolean indicating whether the registration process was successful. Returns true if the registration 
        /// process completes without encountering any errors; otherwise, returns false.
        /// </returns>
        /// <remarks>
        /// This method verifies the entries and cart contents before proceeding with the customer registration process. 
        /// If the cart is empty, an alert is displayed prompting the user to fill it. If the personal data provided is 
        /// invalid, another alert is displayed prompting the user to retry. Upon successful validation of entries and 
        /// cart contents, the customer's data is added to the database. Subsequently, the method retrieves the customer's 
        /// ID and uses it to add the order details to the database. If any errors occur during the registration process, 
        /// appropriate error messages are displayed to the user, and the method returns false. Otherwise, it returns true 
        /// to indicate successful registration.
        /// </remarks>
        public async Task<bool> RegisterCustomer()
        {
            //Check entries & Cart
            bool goNextStep = false;
            if (IsOrderEmpty())
            {
                await Application.Current.MainPage.DisplayAlert("Empty Cart", "Fill it", "OK");
                _parentPage.ClosePopup();
            }
            else if (IsDataInvalid())
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Personal Data", "Retry", "OK");
            }
            else
            {
                Dictionary<string, object> dataCustomer = new Dictionary<string, object> {
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
                    idCustomer = customerDataList[customerDataList.Count - 1]["idCustomer"].ToString();
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
                    //Fail to retrieve data from DB
                    List<Dictionary<string, string>> orderDataList = await databaseOrder.GetData(dataOrderString);
                    if (dataOrderString.Count <= 0 || orderDataList == null)
                    {
                        orderDataList = await databaseOrder.GetData(dataOrderString);
                        await Application.Current.MainPage.DisplayAlert("ERROR 2", "We encounter problems trying to get your order registered", "OK");
                    }
                    else
                    {
                        idOrder = orderDataList[orderDataList.Count - 1]["idOrder"].ToString();
                        goNextStep = true;
                    }
                }
            }
            return goNextStep;
        }

        /// <summary>
        /// Registers the items in the customer's cart, including cabinets and lockers, in the database.
        /// </summary>
        /// <param name="goToCartStep">A boolean value indicating whether to proceed with the cart registration process.</param>
        /// <returns>True if the cart registration process completes successfully; otherwise, false.</returns>
        /// <remarks>
        /// This method iterates over each item in the customer's cart and registers the cabinets and their corresponding 
        /// lockers in the database. It constructs dictionaries containing the necessary information for cabinet and locker 
        /// registration, such as price, dimensions, quantity, references, etc. Each cabinet and its associated lockers are 
        /// added to the database asynchronously. If any errors occur during the registration process, appropriate error 
        /// messages are displayed, and the method returns false. Otherwise, it returns true upon successful registration.
        /// </remarks>
        private async Task<bool> RegisterCart(bool goToCartStep)
        {
            bool cartRegistered = false;
            for(int i = 0; i < Cart.Count(); i++)
            {
                Dictionary<string, object> cabinetToBeRegistered = new Dictionary<string, object>
                { { "price",Cart[i].Price},
                { "width",Cart[i].Length},
                { "height",Cart[i].Height},
                {"quantity",Cart[i].Quantity},
                {"idOrder",idOrder},
                {"IronAngleRef", await Cart[i].Cabinet.GetObservableLockers()[Cart[i].Cabinet.GetObservableLockers().Count()-1].GetCatalogRef("COR")} };

                await databaseCabinet.Add(cabinetToBeRegistered);

                //Get CabinetID from DB
                Dictionary<string, string> dataCabinetString = new Dictionary<string, string>();

                foreach (var kvp in cabinetToBeRegistered)
                {
                    dataCabinetString.Add(kvp.Key, kvp.Value.ToString());
                }
                List<Dictionary<string, string>> cabinetDataList = await databaseCabinet.GetData(dataCabinetString);

                if (dataCabinetString.Count <= 0 || cabinetDataList == null)
                {
                    cabinetDataList = await databaseCabinet.GetData(dataCabinetString);
                    await Application.Current.MainPage.DisplayAlert("ERROR 3", "We encounter problems trying to get your order registered", "OK"); 
                }
                else
                {
                    idCabinet = cabinetDataList[cabinetDataList.Count- 1]["idCabinet"].ToString();
                    Debug.WriteLine(Cart[i].Cabinet.GetObservableLockers().Count());

                    // Register every Locker of the Cabinet in the DB
                    for(int j = 0; j < Cart[i].Cabinet.GetObservableLockers().Count(); j++)
                    {
                        Debug.WriteLine("***** Loop Lockers Regestery *****");

                        Dictionary<string, object> lockerToBeRegistered = new Dictionary<string, object>
                        { 
                        { "height", Cart[i].Cabinet.GetObservableLockers()[j].Height.ToString()},
                        { "color",Cart[i].Cabinet.GetObservableLockers()[j].Color.ToString()},
                        { "door",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("DOORBOOL")},
                        { "price",Cart[i].Cabinet.GetObservableLockers()[j].Price.ToString()},
                        { "idCabinet",idCabinet},
                        { "sidePanelRef",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("PAG")},
                        { "verticalBattenRef",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("TAS")},
                        { "backPanelRef",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("PAR")},
                        { "horizontalPanelRef",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("PAH")},
                        { "doorRef",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("POR")},
                        { "sideCrossbarRef",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("TRG")},
                        { "frontCrossbarRef",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("TRF")},
                        { "backCrossbarRef",await Cart[i].Cabinet.GetObservableLockers()[j].GetCatalogRef("TRR")}
                        };

                        //Debug
                        foreach (var kvp in lockerToBeRegistered)
                        {
                            Debug.WriteLine(kvp.Value.ToString());
                            Debug.WriteLine(kvp.Key.ToString());
                            Debug.WriteLine("_________");
                        }
                        await databaseLocker.Add(lockerToBeRegistered);

                        if (j >= Cart[i].Cabinet.GetObservableLockers().Count()-1 && i >= Cart.Count() - 1)
                        {
                            cartRegistered = true;
                        }
                    }
                }    
            }
            return cartRegistered;
        } 
    }
}

using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using Kitbox_project.Models;
using Kitbox_project.DataBase;
using Kitbox_project.Utilities;
using System.Diagnostics;
using System.Linq;
using Microsoft.Maui.Controls.Compatibility;
using System.Windows.Input;
using System.ComponentModel;
using Syncfusion.Maui.Core.Carousel;
//using Java.Lang;

namespace Kitbox_project.Views
{
    public partial class CabinetCreatorPage : ContentPage
    {
        private CabinetViewModel _viewModel;
        private LogOutViewModel _logOutViewModel;

        private Order _order;
        public Order Order
        { get => _order; set => _order = value; }

        private Cabinet _cabinet;
        public Cabinet Cabinet
        { get => _cabinet; set => _cabinet = value; }

        private int _IDCabinet;
        public int IDCabinet
        { get => _IDCabinet; set => _IDCabinet = value; }

        private Order order;

        private int _totalSize;
        public int TotalSize
        {
            get => _totalSize;
            set
            {
                _totalSize = value;
                OnPropertyChanged(); // Implement INotifyPropertyChanged if needed
            }
        }

        private float _totalPrice;
        public float TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(); // Implement INotifyPropertyChanged if needed
            }
        }


        int indexLock = 0;

        private List<Dictionary<string, int>> registeredPartsRefQuantity = new List<Dictionary<string, int>>();
        public CabinetCreatorPage(Order Order, List<Dictionary<string, int>> registeredPartsRefQuantity = null)
        {
            this.order = Order;
            if (registeredPartsRefQuantity != null) { this.registeredPartsRefQuantity = registeredPartsRefQuantity; }

            InitializeComponent();

            _viewModel = new CabinetViewModel(registeredPartsRefQuantity);
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            _logOutViewModel = new LogOutViewModel();

            BindingContext = _viewModel;
            LogOutButton.BindingContext = _logOutViewModel;


            // Load available lockers into the view model
            LoadAvailableLockers();
            DisablePickers();
            calculateTotalSize();

        }

        private void LoadAvailableLockers()
        {
            _viewModel.AvailableLockers = new ObservableCollection<LockerViewModel> {};
        }


        public void LockerDepthPickerFocused(object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Depth");
        }
        void DoorMaterialPickerFocused (object sender, FocusEventArgs e)
        {
            if (_viewModel.SelectedDoorColorItem != null) 
            {
                _viewModel.SelectedDoorColorItem = null;
                _viewModel.UpdatePickerList("Door_color"); 
            }
            _viewModel.UpdatePickerList("Door_material");
        }
        void DoorColorPickerFocused (object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Door_color");
        }
        void LockerHeightPickerFocused (object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Height");
        }
        void LockerWidthPickerFocused (object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Width");
        }
        void LockerColorPickerFocused (object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Panel_color");
        }
        void AngleIronColorPickerFocused (object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Angle_color");
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.AvailableLockers))
            {
                DisablePickers();
                calculateTotalSize();
            }
        }

        private async void OnDeleteLockerClicked(object sender, EventArgs e)
        {
            int index = 1;
            var button = sender as Button;
            var locker = button?.BindingContext as LockerViewModel;
            int delIndex1 = _viewModel.registeredPartsRefQuantityList.Count - _viewModel.AvailableLockers.Count;
            int delCount = _viewModel.AvailableLockers.Count;

            if (locker != null)
            {
                _viewModel.registeredPartsRefQuantityList.RemoveRange(delIndex1,delCount);
                _viewModel.AvailableLockers.Remove(locker); // Remove the locker from the ViewModel          
            }

            foreach (LockerViewModel locke in _viewModel.AvailableLockers)
            {
                //On redonne un bon index � chacun 
                locke.LockerID = index;
                index += 1;

                locke.NotePartsAvailability = await _viewModel.NotePartsAvailabilityAsync(locke.Locker);

                Debug.WriteLine(locke.LockerID);
                Debug.WriteLine(locke.Locker.ToString());
                Debug.WriteLine(locke.NotePartsAvailability);
            }
            DisablePickers();
            calculateTotalSize();


        }

        private async void AddSelectedLocker_Clicked(object sender, EventArgs e)
        {
            // Check if the maximum number of lockers has been reached
            if (_viewModel.AvailableLockers.Count >= 7 )
            {
                // Display an alert or perform any other action
                return;
            }
            //Check if pickers are correctly completed
            if (_viewModel.selectedValues.ContainsValue(null))
            {
                if (!_viewModel.IsDoorChecked)
                {
                    Dictionary<string, object> data = _viewModel.selectedValues;
                    data.Remove("Door_color");
                    data.Remove("Door_material");
                    if (data.ContainsValue(null))
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            // Create a new LockerViewModel based on the selected parameters
            Door door;
            if (_viewModel.SelectedDoorColorItem != null && _viewModel.SelectedDoorMaterialItem != null)
            {
                door = new Door(_viewModel.SelectedDoorColorItem, 
                    _viewModel.SelectedDoorMaterialItem, 
                    Convert.ToInt32(_viewModel.SelectedHeightItem), 
                    Convert.ToInt32(_viewModel.SelectedWidthItem)); // Assuming default material and dimensions
            }
            else
            {
                door = null;
            }
            
            Locker lockerToAdd = new Locker(Convert.ToInt32(_viewModel.SelectedHeightItem),
                                            Convert.ToInt32(_viewModel.SelectedDepthItem),
                                            Convert.ToInt32(_viewModel.SelectedWidthItem),
                                            _viewModel.SelectedLockerColorItem,
                                            door,
                                            0);

            LockerViewModel newLocker = new LockerViewModel
            {
                Locker = lockerToAdd,
                Height = Convert.ToInt32(_viewModel.SelectedHeightItem),
                Color = _viewModel.SelectedLockerColorItem,
                Door = door,
                NotePartsAvailability = await _viewModel.NotePartsAvailabilityAsync(lockerToAdd)
            };

            int index = _viewModel.AvailableLockers.Count + 1;

            // Set the locker ID as the generated index
            newLocker.LockerID = index;
            // Add the new locker to the AvailableLockers collection
            _viewModel.AvailableLockers.Add(newLocker);
            DisablePickers();
            calculateTotalSize();
            calculateTotalPrice();
        }

        private async void ModifySelectedLocker_Clicked(object sender, EventArgs e)
        {
            if( indexLock is not 0)
            {            
                var locker = _viewModel.AvailableLockers[indexLock - 1];
                Debug.WriteLine(locker);
                locker.Color = _viewModel.SelectedLockerColorItem;
                locker.Height = Convert.ToInt32(_viewModel.SelectedHeightItem);
                Door door = new Door(_viewModel.SelectedDoorColorItem, _viewModel.SelectedDoorMaterialItem, Convert.ToInt32(_viewModel.SelectedHeightItem), Convert.ToInt32(_viewModel.SelectedWidthItem)); // Assuming default material and dimensions
                locker.Door = door;
                _viewModel.registeredPartsRefQuantityList[_viewModel.registeredPartsRefQuantityList.Count - _viewModel.AvailableLockers.Count - 1 + indexLock] = new Dictionary<string, int>();
                locker.NotePartsAvailability = await _viewModel.NotePartsAvailabilityAsync(locker.Locker, indexLock);

            }

            if(indexLock is 0)
            {
                Debug.WriteLine("Please Select a locker");
            }
        }
        private void calculateTotalSize()
        {
            // Convert ObservableCollection<LockerViewModel> to List<Locker>
            List<Locker> lockers = _viewModel.AvailableLockers.Select(viewModel => new Locker(
                Convert.ToInt32(viewModel.Height),
                Convert.ToInt32(_viewModel.SelectedDepthItem),
                Convert.ToInt32(_viewModel.SelectedWidthItem),
                viewModel.Color,
                viewModel.Door != null ? new Door(viewModel.Door.Color, viewModel.Door.Material, Convert.ToInt32(_viewModel.SelectedWidthItem), Convert.ToInt32(_viewModel.SelectedHeightItem)) : null,
                0 // Price
            )).ToList();

            TotalSize = lockers.Sum(locker => locker.Height);
        Debug.WriteLine(TotalSize);
        }

        private async  void calculateTotalPrice()
        {
            TotalPrice = 0;
            foreach (LockerViewModel locker in _viewModel.AvailableLockers)
            {
                TotalPrice += Convert.ToSingle(await locker.Locker.GetPrice());
                Math.Round(TotalPrice, 2);
            }
        }


        private void DisablePickers()
        {
            if (_viewModel.AvailableLockers.Count() == 0)
            {
                CabinetWidth.IsEnabled = true;
                CabinetDepth.IsEnabled = true;
                AngleIronColor.IsEnabled = true;
            }
            else
            {
                CabinetWidth.IsEnabled = false;
                CabinetDepth.IsEnabled = false;
                AngleIronColor.IsEnabled = false;
            }
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var locker = button.BindingContext as LockerViewModel;
                if (locker != null)
                {
                    indexLock = locker.LockerID;

                    _viewModel.SelectedLockerColorItem = locker.Color;
                    _viewModel.SelectedHeightItem = Convert.ToString(locker.Height);
                    if(locker.Door != null && locker.Door.Color is not null)
                    {
                        _viewModel.SelectedDoorColorItem = locker.Door.Color;
                        _viewModel.SelectedDoorMaterialItem = locker.Door.Material;
                    }
                }
                else
                {
                    Debug.WriteLine("Error: No locker selected.");
                }
            }
        }


        private async void OnConfimButtonClicked (object sender, EventArgs e)
        {
            // Convert ObservableCollection<LockerViewModel> to List<Locker>
            List<Locker> lockers = _viewModel.AvailableLockers.Select(viewModel => new Locker(
                Convert.ToInt32(viewModel.Height),
                Convert.ToInt32(_viewModel.SelectedDepthItem),
                Convert.ToInt32(_viewModel.SelectedWidthItem),
                viewModel.Color,
                viewModel.Door != null ? new Door(viewModel.Door.Color, viewModel.Door.Material, Convert.ToInt32(_viewModel.SelectedWidthItem), Convert.ToInt32(_viewModel.SelectedHeightItem)) : null,
                Math.Round(Convert.ToDouble(viewModel.Locker.Price),2)
            )).ToList();

            TotalSize = lockers.Sum(locker => locker.Height);
            Debug.WriteLine("La taille totale est");
            Debug.WriteLine(TotalSize);


            //On cr�e un nouveau cabinet
            Cabinet newCabinet = new Cabinet(
                lockers,
                Convert.ToInt32(_viewModel.SelectedDepthItem),
                Convert.ToInt32(_viewModel.SelectedWidthItem),
                TotalSize, // Height pour le moment mais faudra remplacer par angle iron
                1
            ); 

            // Add the new Cabinet to the Order's cart

            if (order.Cart.Count() != 0)
            {
                int newIndex = order.Cart.Last().CabinetID + 1;

                newCabinet.CabinetID = newIndex;
            }

            else
            {
                newCabinet.CabinetID = 0;
            }

            Debug.WriteLine(newCabinet.ToString());

            order.Cart.Add(newCabinet);

            // Create the cart page
            CartPage newCartPage = new CartPage(order, _viewModel.registeredPartsRefQuantityList);

            // Make the cart page visible
            await Navigation.PushAsync(newCartPage);
        }
        
    }
}

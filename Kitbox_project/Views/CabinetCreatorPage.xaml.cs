using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using Kitbox_project.Models;
using Kitbox_project.DataBase;
using Kitbox_project.Utilities;
using System.Diagnostics;
using System.Linq;
using Microsoft.Maui.Controls.Compatibility;
using System.Windows.Input;
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


        int indexLock = 0;
        int totalSize = 0;

        public CabinetCreatorPage(Order Order)
        {
            order = Order;

            InitializeComponent();
            _viewModel = new CabinetViewModel();
            _logOutViewModel = new LogOutViewModel();

            BindingContext = _viewModel;
            LogOutButton.BindingContext = _logOutViewModel;


            // Load available lockers into the view model
            LoadAvailableLockers();
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


        private void OnDeleteLockerClicked(object sender, EventArgs e)
        {
            int index = 1;
            var button = sender as Button;
            var locker = button?.BindingContext as LockerViewModel;
            if (locker != null)
            {
                _viewModel.AvailableLockers.Remove(locker); // Remove the locker from the ViewModel
            }

            foreach (var locke in _viewModel.AvailableLockers)
            {
                //On redonne un bon index à chacun 
                locke.LockerID = index;
                index += 1;
            }
            //DisablePickers();

        }

        private void AddSelectedLocker_Clicked(object sender, EventArgs e)
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
                        Debug.WriteLine(" -- Empty Pickers !! -- ");
                        return;
                    }
                }
                else
                {
                    Debug.WriteLine(" -- Empty Pickers !! -- ");
                    return;
                }

            }

            // Create a new LockerViewModel based on the selected parameters
            Door door = new Door(_viewModel.SelectedDoorColorItem, _viewModel.SelectedDoorMaterialItem, Convert.ToInt32(_viewModel.SelectedHeightItem), Convert.ToInt32(_viewModel.SelectedWidthItem)); // Assuming default material and dimensions

            LockerViewModel newLocker = new LockerViewModel
            {
                Height = Convert.ToInt32(_viewModel.SelectedHeightItem),
                Color = _viewModel.SelectedLockerColorItem,
                Door = door,
                NotePartsAvailability = _viewModel.NotePartsAvailability()
            };

            int index = _viewModel.AvailableLockers.Count + 1;

            // Set the locker ID as the generated index
            newLocker.LockerID = index;
            // Add the new locker to the AvailableLockers collection
            _viewModel.AvailableLockers.Add(newLocker);
            System.Diagnostics.Debug.WriteLine(_viewModel.AvailableLockers.Count());
            //DisablePickers();
        }

        private void ModifySelectedLocker_Clicked(object sender, EventArgs e)
        {
            if( indexLock is not 0)
            {

            
            var locker = _viewModel.AvailableLockers[indexLock - 1];
            Debug.WriteLine(locker);
            locker.Color = _viewModel.SelectedLockerColorItem;
            locker.Height = Convert.ToInt32(_viewModel.SelectedHeightItem);
            Door door = new Door(_viewModel.SelectedDoorColorItem, _viewModel.SelectedDoorMaterialItem, Convert.ToInt32(_viewModel.SelectedHeightItem), Convert.ToInt32(_viewModel.SelectedWidthItem)); // Assuming default material and dimensions
            locker.Door = door;
            }

            if(indexLock is 0)
            {
                Debug.WriteLine("Please Select a locker");
            }

        }
/*
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

*/

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
                    if(locker.Door.Color is not null)
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
                new Door(viewModel.Door.Color, viewModel.Door.Material, Convert.ToInt32(_viewModel.SelectedWidthItem), Convert.ToInt32(_viewModel.SelectedHeightItem)), 
                0 // Price
            )).ToList();

            totalSize = lockers.Sum(locker => locker.Height);
            Debug.WriteLine("La taille totale est");
            Debug.WriteLine(totalSize);


            //On crée un nouveau cabinet
            Cabinet newCabinet = new Cabinet(
                lockers,
                Convert.ToInt32(_viewModel.SelectedDepthItem),
                Convert.ToInt32(_viewModel.SelectedWidthItem),
                totalSize, // Height pour le moment mais faudra remplacer par angle iron
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
            CartPage newCartPage = new CartPage(order);

            // Make the cart page visible
            await Navigation.PushAsync(newCartPage);
        }
        
    }
}

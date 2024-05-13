using Kitbox_project.Models;
using Kitbox_project.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Kitbox_project.Views
{
    public partial class EditCabinetPage : ContentPage
    {
        private Cabinet _cabinet;
        private CabinetViewModel _viewModel;
        private LogOutViewModel _logOutViewModel;
        int indexLock = 0;
        int index = 1;
        List<Dictionary<string, int>> registeredPartsRefQuantity = new List<Dictionary<string, int>>();

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


        public EditCabinetPage(Order order, Cabinet cabinet, List<Dictionary<string, int>> registeredPartsRefQuantity = null)
        {
            InitializeComponent();
            _cabinet = cabinet;
            _viewModel = new CabinetViewModel();
            _logOutViewModel = new LogOutViewModel();
            if (registeredPartsRefQuantity != null) { this.registeredPartsRefQuantity = registeredPartsRefQuantity; }   
            BindingContext = _viewModel;
            LogOutButton.BindingContext = _logOutViewModel;
            LoadAvailableLockers();
            DefaultPickers();
            calculateTotalSize();
            calculateTotalPrice();

        }


        public void LockerDepthPickerFocused(object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Depth");
        }
        void DoorMaterialPickerFocused(object sender, FocusEventArgs e)
        {
            if (_viewModel.SelectedDoorColorItem != null)
            {
                _viewModel.SelectedDoorColorItem = null;
                _viewModel.UpdatePickerList("Door_color");
            }
            _viewModel.UpdatePickerList("Door_material");
        }
        void DoorColorPickerFocused(object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Door_color");
        }
        void LockerHeightPickerFocused(object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Height");
        }
        void LockerWidthPickerFocused(object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Width");
        }
        void LockerColorPickerFocused(object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Panel_color");
        }
        void AngleIronColorPickerFocused(object sender, FocusEventArgs e)
        {
            _viewModel.UpdatePickerList("Angle_color");
        }

        private void LoadAvailableLockers()
        {
            _viewModel.AvailableLockers = new ObservableCollection<LockerViewModel>
            {
            };
        }

        private async void DefaultPickers()
        {
            //Debug.WriteLine(_cabinet.AngleIronColor.ToString());
            _viewModel.SelectedDepthItem = _cabinet.Depth.ToString();
            _viewModel.SelectedWidthItem = _cabinet.Length.ToString();
            _viewModel.SelectedAngleIronColor = _cabinet.AngleIronColor;


            //Debug.WriteLine(_viewModel.SelectedDepthItem);
            //Debug.WriteLine(_viewModel.SelectedWidthItem);
            //Debug.WriteLine(_viewModel.SelectedAngleIronColor);


            foreach (var locker in _cabinet.GetObservableLockers())
            {

                Door door;

                if (locker.Door != null)
                {
                    door = new Door(locker.Door.Color, 
                        locker.Door.Material,
                        Convert.ToInt32(locker.Width), 
                        Convert.ToInt32(locker.Height)); // Assuming default material and dimensions
                }
                else
                {
                    door = null;
                }

                //put the needed dictionary at the end of the list
                registeredPartsRefQuantity.Remove(locker.partsRegisteredForLocker);
                registeredPartsRefQuantity.Add(locker.partsRegisteredForLocker);
                _viewModel.registeredPartsRefQuantityList = registeredPartsRefQuantity;



                Locker lockerToAdd = new Locker(Convert.ToInt32(locker.Height),
                                                Convert.ToInt32(locker.Depth),
                                                Convert.ToInt32(locker.Width),
                                                locker.Color,
                                                door,
                                                locker.Price);
                Debug.WriteLine(lockerToAdd);
                LockerViewModel newLocker = new LockerViewModel
                {
                    Locker = lockerToAdd,
                    Height = Convert.ToInt32(locker.Height),
                    Color = locker.Color,
                    Door = door,
                    Price = Convert.ToInt32(locker.Price),

                };
                newLocker.LockerID = index;

                _viewModel.AvailableLockers.Add(newLocker);
                // Display locker details as per your requirement
                Debug.WriteLine($"Locker Color: {locker.Color}, Height: {locker.Height}, Width: {locker.Width}, Depth: {locker.Depth}, Price: {locker.Price}" +
                               (locker.Door != null ? $", Door Color: {locker.Door.Color}" : ""));
                index++;
            }

        }
         
        private async void AddSelectedLocker_Clicked(object sender, EventArgs e)
        {
            // Check if the maximum number of lockers has been reached
            if (_viewModel.AvailableLockers.Count >= 7)
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
            Debug.WriteLine(_viewModel.AvailableLockers.Count());
            calculateTotalSize();
            calculateTotalPrice();

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

        private async void calculateTotalPrice()
        {
            TotalPrice = 0;
            foreach (LockerViewModel locker in _viewModel.AvailableLockers)
            {
                Debug.WriteLine("The locker are");
                Debug.WriteLine(locker.Locker);
                Debug.WriteLine(locker.Locker.Price);
                if (locker != null && locker.Locker != null)
                {
                    if (locker.Locker.Price == 0)
                    {
                        Debug.WriteLine(Convert.ToSingle(await locker.Locker.GetPrice()));
                        TotalPrice += Convert.ToSingle(await locker.Locker.GetPrice());
                    }
                    else
                    {
                        TotalPrice += Convert.ToInt32(locker.Locker.Price);
                    }

                    Math.Round(TotalPrice, 2);

                }
            }
        }



        private void ModifySelectedLocker_Clicked(object sender, EventArgs e)
        {
            if (indexLock is not 0)
            {
                var locker = _viewModel.AvailableLockers[indexLock - 1];
                Debug.WriteLine(locker);
                locker.Color = _viewModel.SelectedLockerColorItem;
                locker.Height = Convert.ToInt32(_viewModel.SelectedHeightItem);
                Door door = new Door(_viewModel.SelectedDoorColorItem, _viewModel.SelectedDoorMaterialItem, Convert.ToInt32(_viewModel.SelectedHeightItem), Convert.ToInt32(_viewModel.SelectedWidthItem)); // Assuming default material and dimensions
                locker.Door = door;
            }

            if (indexLock is 0)
            {
                Debug.WriteLine("Please Select a locker");
            }
            calculateTotalSize();
            calculateTotalPrice();


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
                    if (locker.Door is not null)
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
            calculateTotalSize();

        }

        private void OnDeleteLockerClicked(object sender, EventArgs e)
        {
            index = 1;
            var button = sender as Button;
            var locker = button?.BindingContext as LockerViewModel;
            if (locker != null)
            {
                _viewModel.AvailableLockers.Remove(locker); // Remove the locker from the ViewModel
            }
            //Debug.WriteLine("NOMBRE DE LOCKERS");
            //Debug.WriteLine(_viewModel.AvailableLockers.Count());

            foreach (var locke in _viewModel.AvailableLockers)
            {
                //Debug.WriteLine(locke);
                //Debug.WriteLine(index);
                locke.LockerID = index;
                index +=1;
            }
            calculateTotalSize();
            calculateTotalPrice();

        }


        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {

            // Implement logic to update cabinet details based on user input
            _cabinet.Depth = Convert.ToInt32(_viewModel.SelectedDepthItem);
            _cabinet.Length = Convert.ToInt32(_viewModel.SelectedWidthItem);
            foreach (var locker in _cabinet.GetObservableLockers())
            {
                Debug.WriteLine(locker);
                Debug.WriteLine(locker.Price);
                _cabinet.TestRemoveLocker(locker);  
            }
            _cabinet.GetObservableLockers().Clear();
            Debug.WriteLine(_cabinet.GetObservableLockers().Count());
            List<Locker> lockers = _viewModel.AvailableLockers.Select(viewModel => new Locker(
                            Convert.ToInt32(viewModel.Height),
                            Convert.ToInt32(_viewModel.SelectedDepthItem),
                            Convert.ToInt32(_viewModel.SelectedWidthItem),
                            viewModel.Color,
                            viewModel.Door != null ? new Door(viewModel.Door.Color, viewModel.Door.Material, Convert.ToInt32(_viewModel.SelectedWidthItem), Convert.ToInt32(_viewModel.SelectedHeightItem)) : null,
                            Math.Round(Convert.ToDouble(viewModel.Locker.Price), 2)
                        )).ToList();

            TotalSize = lockers.Sum(locker => locker.Height);
            Debug.WriteLine("La taille totale est");
            Debug.WriteLine(TotalSize);


            foreach (var locker in lockers)
            {
                _cabinet.AddLocker(locker);
            }

            // Navigate back to CartPage after updating
            await Navigation.PopAsync();
        }
    }
}

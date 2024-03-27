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
        int indexLock = 0;
        int index = 1;



        public EditCabinetPage(Order order, Cabinet cabinet)
        {
            InitializeComponent();
            _cabinet = cabinet;
            _viewModel = new CabinetViewModel();
            BindingContext = _viewModel;
            LoadAvailableLockers();
            DefaultPickers();
            
        }

        private void LoadAvailableLockers()
        {



            _viewModel.AvailableLockers = new ObservableCollection<LockerViewModel>
            {

            };
        }

        private void DefaultPickers()
        {
            Debug.WriteLine(_cabinet.Depth.ToString());
            _viewModel.SelectedDepthItem = _cabinet.Depth.ToString();
            _viewModel.SelectedWidthItem = _cabinet.Length.ToString();


            foreach (var locker in _cabinet.GetObservableLockers())
            {
                Door door = new Door(locker.Door.Color, locker.Door.Material, Convert.ToInt32(locker.Width), Convert.ToInt32(locker.Height)); // Assuming default material and dimensions

                LockerViewModel newLocker = new LockerViewModel
                {
                    Height = Convert.ToInt32(locker.Height),
                    Color = locker.Color,
                    Door = door,
                    NotePartsAvailability = "NotePartsAvailability(ARTHUR)"
                };
                newLocker.LockerID = index;

                _viewModel.AvailableLockers.Add(newLocker);
                // Display locker details as per your requirement
                Debug.WriteLine($"Locker Color: {locker.Color}, Height: {locker.Height}, Width: {locker.Width}, Depth: {locker.Depth}, Door Color: {locker.Door.Color}");
                index++;
            }

        }
         
        private void AddSelectedLocker_Clicked(object sender, EventArgs e)
        {
            // Check if the maximum number of lockers has been reached
            if (_viewModel.AvailableLockers.Count >= 7)
            {
                // Display an alert or perform any other action
                return;
            }

            // Create a new LockerViewModel based on the selected parameters
            Door door = new Door(_viewModel.SelectedDoorColorItem, _viewModel.SelectedDoorMaterialItem, Convert.ToInt32(_viewModel.SelectedHeightItem), Convert.ToInt32(_viewModel.SelectedWidthItem)); // Assuming default material and dimensions

            LockerViewModel newLocker = new LockerViewModel
            {
                Height = Convert.ToInt32(_viewModel.SelectedHeightItem),
                Color = _viewModel.SelectedLockerColorItem,
                Door = door,
                NotePartsAvailability = "NotePartsAvailability(ARTHUR)"
            };

            int index = _viewModel.AvailableLockers.Count + 1;

            // Set the locker ID as the generated index
            newLocker.LockerID = index;
            // Add the new locker to the AvailableLockers collection
            _viewModel.AvailableLockers.Add(newLocker);
            Debug.WriteLine(_viewModel.AvailableLockers.Count());
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
                    if (locker.Door.Color is not null)
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
        }


        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {

            // Implement logic to update cabinet details based on user input

            //Debug.WriteLine(Convert.ToInt32(CabinetWidth.SelectedItem));
            //Debug.WriteLine(Convert.ToInt32(CabinetDepth.SelectedItem));
            _cabinet.Depth = Convert.ToInt32(_viewModel.SelectedDepthItem);
            _cabinet.Length = Convert.ToInt32(_viewModel.SelectedWidthItem);
            foreach (var locker in _cabinet.GetObservableLockers())
            {
                _cabinet.TestRemoveLocker(locker);  
            }


            _cabinet.GetObservableLockers().Clear();
            List<Locker> lockers = _viewModel.AvailableLockers.Select(viewModel => new Locker(
                Convert.ToInt32(viewModel.Height),
                Convert.ToInt32(_viewModel.SelectedDepthItem),
                Convert.ToInt32(_viewModel.SelectedWidthItem),
                viewModel.Color,
                new Door(viewModel.Door.Color, viewModel.Door.Material, Convert.ToInt32(_viewModel.SelectedWidthItem), Convert.ToInt32(_viewModel.SelectedHeightItem)),
                0 // Price
            )).ToList();
            

            foreach (var locker in lockers)
            {
                _cabinet.AddLocker(locker);
            }

            // Navigate back to CartPage after updating
            await Navigation.PopAsync();
        }
    }
}

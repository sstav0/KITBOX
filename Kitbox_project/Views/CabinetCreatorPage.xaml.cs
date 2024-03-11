using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using Kitbox_project.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Kitbox_project.Utilities;
using System.Diagnostics;

namespace Kitbox_project.Views
{
    public partial class CabinetCreatorPage : ContentPage
    {
        private CabinetViewModel _viewModel;

        private Order _order;
        public Order Order
        { get => _order; set => _order = value; }

        private Cabinet _cabinet;
        public Cabinet Cabinet
        { get => _cabinet; set => _cabinet = value; }

        private int _IDCabinet;
        public int IDCabinet
        { get => _IDCabinet; set => _IDCabinet = value; }


        int indexLock = 0;

        public CabinetCreatorPage()
        {
            InitializeComponent();
            _viewModel = new CabinetViewModel();
            BindingContext = _viewModel;

            // Load available lockers into the view model
            LoadAvailableLockers();
        }



        private void LoadAvailableLockers()
        {



            _viewModel.AvailableLockers = new ObservableCollection<LockerViewModel>
            {

            };
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
            Door door = new Door(_viewModel.SelectedDoorColorItem, "Wood", 50, 40); // Assuming default material and dimensions

            LockerViewModel newLocker = new LockerViewModel
            {
                Height = Convert.ToInt32(_viewModel.SelectedHeightItem),
                Color = _viewModel.SelectedLockerColorItem,
                Door = door

            };

            int index = _viewModel.AvailableLockers.Count + 1;

            // Set the locker ID as the generated index
            newLocker.LockerID = index;
            // Add the new locker to the AvailableLockers collection
            _viewModel.AvailableLockers.Add(newLocker);
            System.Diagnostics.Debug.WriteLine(_viewModel.AvailableLockers.Count());
        }

        private void ModifySelectedLocker_Clicked(object sender, EventArgs e)
        {
            if( indexLock is not 0)
            {

            
            var locker = _viewModel.AvailableLockers[indexLock - 1];
            Debug.WriteLine(locker);
            locker.Color = _viewModel.SelectedLockerColorItem;
            locker.Height = Convert.ToInt32(_viewModel.SelectedHeightItem);
            Door door = new Door(_viewModel.SelectedDoorColorItem, "Wood", 50, 40); // Assuming default material and dimensions
            locker.Door = door;
            }

            if(indexLock is 0)
            {
                Debug.WriteLine("Please Select a locker");
            }

        }


        private void OnAddLockerButtonClicked(object sender, EventArgs e) 
        { 
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
                    if(locker.Door.Color is not null)
                    {
                    _viewModel.SelectedDoorColorItem = locker.Door.Color;

                    }


    
                }
                else
                {
                    Debug.WriteLine("Error: No locker selected.");
                }
            }
        }




        private void OnConfimButtonClicked (object sender, EventArgs e)
        {
            // Convert ObservableCollection<LockerViewModel> to List<Locker>
            List<Locker> lockers = _viewModel.AvailableLockers.Select(viewModel => new Locker(
                viewModel.LockerID,
                Convert.ToInt32(viewModel.Height),
                0,
                viewModel.Color,
                new Door(viewModel.Door.Color, "Wood", 50, 40), // Prcq on a tjr pas mis les materiaux pour la porte je mets au pif rn 
                0 // Price
            )).ToList();

            //On crée un nouveau cabinet
            Cabinet newCabinet = new Cabinet(
                lockers,
                Convert.ToInt32(_viewModel.SelectedDepthItem),
                Convert.ToInt32(_viewModel.SelectedWidthItem),
                1 // Height pour le moment mais faudra remplacer par angle iron
            );

            // Add the new Cabinet to the Order's cart
            Debug.WriteLine(newCabinet.ToString());
            //Cabinet newCabinet = new Cabinet(_viewModel.AvailableLockers, _viewModel.SelectedDepthItem, _viewModel.SelectedWidthItem, 1, 1);
            //System.Diagnostics.Debug.WriteLine(newCabinet);
        }
        
    }
}

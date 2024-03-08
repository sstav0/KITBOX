using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using Kitbox_project.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Kitbox_project.Utilities;

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
            // Few randoms lockers I created to test until we get the DB ready. 
            // To delete once we've created the DB 

            Door door1 = new Door ( "Red", "Wood",50, 40);
            Door door2 = new Door("None", "Glass", 50, 30);
            Door door3 = new Door("Blue", "Wood", 50, 30);


            _viewModel.AvailableLockers = new ObservableCollection<LockerViewModel>
            {
                new LockerViewModel { Height = 50, Color = "Red", Door = door1, Price = 500, LockerID= 1, NotePartsAvailability="All Right"},
                new LockerViewModel { Height =50,Color= "Blue", Door = door2 , Price = 1000, LockerID=2, NotePartsAvailability = "Everything All Right" },
                new LockerViewModel { Height = 70, Color = "Green", Door = door3 , Price = 250, LockerID=3, NotePartsAvailability = "NOK" },
                

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


        private void OnAddLockerButtonClicked(object sender, EventArgs e) 
        { 
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CabinetViewModel selectedCabinetView)
            {
                
            }
        }
        
        private void OnConfimButtonClicked (object sender, EventArgs e)
        {
            //Cabinet newCabinet = new Cabinet(_viewModel.AvailableLockers, _viewModel.SelectedDepthItem, _viewModel.SelectedWidthItem, 1, 1);
            //System.Diagnostics.Debug.WriteLine(newCabinet);
        }
        
    }
}

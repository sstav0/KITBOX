using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using Kitbox_project.Models;
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

        private Order order;


        int indexLock = 0;

        public CabinetCreatorPage(Order Order)
        {
            order = Order;

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
            var locker = _viewModel.AvailableLockers[indexLock-1];
            Debug.WriteLine(locker);
            locker.Color = _viewModel.SelectedLockerColorItem;
            locker.Height = Convert.ToInt32(_viewModel.SelectedHeightItem);
            locker.Door.Color = _viewModel.SelectedDoorColorItem;

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
                    _viewModel.SelectedDoorColorItem = locker.Door.Color;

    
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
                viewModel.LockerID,
                Convert.ToInt32(viewModel.Height),
                0,
                viewModel.Color,
                new Door(viewModel.Door.Color, "Wood", 50, 40), // Assuming default material and dimensions for the door
                0 // Price
            )).ToList();

            // Create a new Cabinet object
            Cabinet newCabinet = new Cabinet(
                lockers,
                Convert.ToInt32(_viewModel.SelectedDepthItem),
                Convert.ToInt32(_viewModel.SelectedWidthItem),
                1 // Height
            );

            // Add the new Cabinet to the Order's cart
            Debug.WriteLine(newCabinet.ToString());
            order.Cart.Add(newCabinet);

            // Create the cart page
            CartPage newCartPage = new CartPage(order);

            // Make the cart page visible
            await Navigation.PushAsync(newCartPage);
        }
        
    }
}

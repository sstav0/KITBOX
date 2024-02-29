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

        public CabinetCreatorPage()
        {
            InitializeComponent();
            _viewModel = new CabinetViewModel();
            BindingContext = _viewModel;

            // Load available lockers into the view model
            LoadAvailableLockers();
            ShowExpansionPanels();


        }

        private void LoadAvailableLockers()
        {
            // Few randoms lockers I created to test until we get the DB ready. 
            // To delete once we've created the DB 

            _viewModel.AvailableLockers = new ObservableCollection<LockerViewModel>
            {
                new LockerViewModel { Height = 50, Color = "Red", Door = true, Price = 500 },
                new LockerViewModel { Height = 60, Color = "Blue", Door = false, Price = 1000 },
                new LockerViewModel { Height = 70, Color = "Green", Door = true, Price = 250 },

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
            LockerViewModel newLocker = new LockerViewModel
            {
                Height = _viewModel.SelectedHeightItem,
                Door = _viewModel.IsDoorChecked,
                Color = _viewModel.SelectedLockerColorItem
            };

            // Add the new locker to the AvailableLockers collection
            _viewModel.AvailableLockers.Add(newLocker);
            System.Diagnostics.Debug.WriteLine(_viewModel.AvailableLockers.Count());

            // Notify the UI that the collection has changed
            OnPropertyChanged(nameof(_viewModel.AvailableLockers));
            ShowExpansionPanels();
        }

        private void ShowExpansionPanels()
        {
            // Determine the number of available lockers
            int numberOfLockers = _viewModel.AvailableLockers.Count;

            // Loop through each ExpansionPanel and set its visibility
            for (int i = 0; i < numberOfLockers; i++)
            {
                // Find the ExpansionPanel control by its name
                var expansionPanel = FindByName("ExpansionPanel" + (i + 1));

                // Cast the result to ExpansionPanel
                if (expansionPanel is ExpansionPanel panel)
                {
                    // Toggle visibility based on whether the item exists in the list
                    panel.IsVisible = true; // or false depending on your condition
                }
            }
        }

        private void OnAddLockerButtonClicked(object sender, EventArgs e) 
        { 
        }
    }
}

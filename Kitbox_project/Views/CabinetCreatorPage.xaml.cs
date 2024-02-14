using Kitbox_project.ViewModels;
using System.Collections.ObjectModel;
using Kitbox_project.Models;

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
        }

        private void LoadAvailableLockers()
        {
            // Load your available lockers into the view model
            // For example:
            _viewModel.AvailableLockers = new ObservableCollection<LockerViewModel>
            {
                new LockerViewModel { Height = 50, Color = "Red", Door = true, Price = 500 },
                new LockerViewModel { Height = 60, Color = "Blue", Door = false, Price = 1000 },
                new LockerViewModel { Height = 70, Color = "Green", Door = true, Price = 250}
            };
        }

        private void AddSelectedLocker_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.Lockers.Count >= 7)
            {
                return;
            }
            // Add the selected locker to the cabinet
            LockerViewModel selectedLocker = _viewModel.SelectedLocker;
            if (selectedLocker != null)
            {
                _viewModel.Lockers.Add(selectedLocker);
                System.Diagnostics.Debug.WriteLine(_viewModel.Lockers.Count);
                // Optionally, clear the selection in the picker
                lockerPicker.SelectedItem = null;
            }
        }
    }
}

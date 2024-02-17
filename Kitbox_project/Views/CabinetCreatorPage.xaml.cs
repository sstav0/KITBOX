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
            // Few randoms lockers I created to test until we get the DB ready. 
            // To delete once we've created the DB 

            _viewModel.AvailableLockers = new ObservableCollection<LockerViewModel>
            {
                new LockerViewModel { Height = 50, Color = "Red", Door = true, Price = 500 },
                new LockerViewModel { Height = 60, Color = "Blue", Door = false, Price = 1000 },
                new LockerViewModel { Height = 70, Color = "Green", Door = true, Price = 250}
            };

            //Color brown = new Color(139, 69, 19);

            //Door door1 = new Door(brown, "wood");
            //Locker locker1 = new Locker(45, brown, door1, 100);
            //Lockers.Add(locker1);

            //Color white = new Color(0, 0, 0);

            //Door door2 = new Door(white, "glass");
            //Locker locker2 = new Locker(50, white, door2, 100);
            //Lockers.Add(locker2);

            //ListLockers.ItemsSource = Lockers;
        }

        private void AddSelectedLocker_Clicked(object sender, EventArgs e)
        {
            // Si on a plus de 7 lockers ça fait rien 
            if (_viewModel.Lockers.Count >= 7)
            {
                return;
            }
            // On ajoute le locker choisi au cabinet
            LockerViewModel selectedLocker = _viewModel.SelectedLocker;
            if (selectedLocker != null)
            {
                _viewModel.Lockers.Add(selectedLocker);
                System.Diagnostics.Debug.WriteLine(_viewModel.Lockers.Count);
                // On remets le picker à 0 parce que c'est plus cool
                //lockerPicker.SelectedItem = null;
            }
        }

        private void OnAddLockerButtonClicked(object sender, EventArgs e) { }
    }
}

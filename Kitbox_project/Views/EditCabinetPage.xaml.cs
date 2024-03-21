using Kitbox_project.Models;
using System;

namespace Kitbox_project.Views
{
    public partial class EditCabinetPage : ContentPage
    {
        private Cabinet _cabinet;

        public EditCabinetPage(Order order, Cabinet cabinet)
        {
            InitializeComponent();
            _cabinet = cabinet;
            PopulateCabinetDetails();
        }

        private void PopulateCabinetDetails()
        {
            // Populate view with details from _cabinet object
            // Example:
            CabinetWidth.Text = Convert.ToString(_cabinet.Height);
            CabinetDepth.Text = Convert.ToString(_cabinet.Depth);
            // ...
        }

        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            // Implement logic to update cabinet details based on user input
            // Example:
            // _cabinet.Lockers[0].Color = LockerColorEntry.Text;
            // _cabinet.Lockers[0].Height = Convert.ToInt32(LockerHeightEntry.Text);
            // ...
            _cabinet.Length = Convert.ToInt32(CabinetWidth.Text);
            _cabinet.Depth = Convert.ToInt32(CabinetDepth.Text);
            // Navigate back to CartPage after updating
            await Navigation.PopAsync();
        }
    }
}

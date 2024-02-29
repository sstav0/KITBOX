using Kitbox_project.Models;
using Kitbox_project.DataBase;
using System.Diagnostics;

namespace Kitbox_project.Views;

public partial class LockerCreatorPage : ContentPage
{
	
	private bool selectColorEnabler = true;

	
    public LockerCreatorPage()
	{
		InitializeComponent();
		doorPicker.IsVisible = false;
        glassDoor.IsVisible = false;
    }

	private List<string> LoadDoors()
	{
		List<string> doors = new List<string>();
        List<Door> doorList = DatabaseDoorObject.GetList();
		foreach (Door door in doorList)
		{
			doors.Add(door.GetColor().ToString());
		}
        return doors;
    }
    private void OnAddDoorClicked(object sender, CheckedChangedEventArgs e)
	{
        if (addDoor.IsChecked && !glassDoor.IsChecked)
        {
            selectColorEnabler = true;
            glassDoor.IsVisible = true;
        }
        else
        {
            selectColorEnabler = false;
            glassDoor.IsVisible = false;
            Debug.WriteLine("false");
        }
        ShowColorPicker();
    }
    private void OnGlassDoorClicked(object sender, CheckedChangedEventArgs e)
	{
        if (glassDoor.IsChecked)
        {
            selectColorEnabler = false;
            Debug.WriteLine("false");
        }
        else if (addDoor.IsChecked)
        {
            selectColorEnabler = true;
        }
        ShowColorPicker();
    }
    private void ShowColorPicker()
    {
        if (selectColorEnabler)
        {
            doorPicker.IsVisible = true;
            doorPicker.ItemsSource = LoadDoors();
        }
        else
        {
            doorPicker.IsVisible= false;
        }
    }

}
﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kitbox_project.ViewModels;
using System.Diagnostics;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using System.Windows.Input;
using System.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Cryptography.X509Certificates;
//using static CoreFoundation.DispatchSource;
//using Microsoft.UI.Xaml.Controls;

namespace Kitbox_project.ViewModels
{
    public class CabinetViewModel : INotifyPropertyChanged 
    {
        public ICommand OnAddLockerButtonClicked { get; }
        public ICommand OnResetLockerButtonClicked { get; } 

        //private static Color color1 = new Color(255, 152, 12);
        //private static Color color2 = new Color(255, 100, 100);
        private static Door door1 = new Door("color1", "wood", 12, 12);
        private static Door door2 = new Door("color2", "wood", 12, 12);
        private static Locker locker1 = new Locker(12, 12, 12, "Lyla", door1, 45.4);
        private static Locker locker2 = new Locker(12, 12, 10, "Purple", door1, 45.4);
        private static Locker locker3 = new Locker(12, 12, 15, "Purple", door1, 45.4);




        private List<Door> doors = new List<Door>();
        private List<Locker> lockers = new List<Locker>();

        private List<Door> availableDoor = new List<Door>();
        private List<Locker> availableLocker = new List<Locker>();

        private List<Door> allDoor = new List<Door>();
        private List<Locker> allLocker = new List<Locker>();

        private bool selectColorEnabler = true;

        DatabaseCatalog databaseCatalog = new DatabaseCatalog("storekeeper", "storekeeper");


        public CabinetViewModel()
        {

            availableDoor.Add(door1);
            availableDoor.Add(door2);

            availableLocker.Add(locker1);
            availableLocker.Add(locker2);
            availableLocker.Add(locker3);

            allDoor = availableDoor;
            allLocker = availableLocker;

            Lockers = new ObservableCollection<LockerViewModel>();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalPrice();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalSize();
            OnAddLockerButtonClicked = new Command(ExecuteOnAddLockerButtonClicked);
            OnResetLockerButtonClicked = new Command(ExecuteOnResetLockerButtonClicked);

            ResetLocker();

            UpdateAvailability();
        }

        private ObservableCollection<LockerViewModel> _lockers;
        public ObservableCollection<LockerViewModel> Lockers
        {
            get => _lockers;
            set
            {
                Debug.WriteLine("Setting Lockers property...");

                _lockers = value;
                OnPropertyChanged();
                CalculateTotalSize(); // Pas sûr d'en avoir besoin si j'ai mes Lockers.CollectionChanged quelques lignes au dessus
                CalculateTotalPrice();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<LockerViewModel> _availableLockers;
        public ObservableCollection<LockerViewModel> AvailableLockers
        {
            get => _availableLockers;
            set
            {
                _availableLockers = value;
                OnPropertyChanged();
            }
        }

        private LockerViewModel _selectedLocker;
        public LockerViewModel SelectedLocker
        {
            get => _selectedLocker;
            set 
            {
                _selectedLocker = value;
                OnPropertyChanged();
            }
        }
        //Checkboxes features
        private bool _isDoorChecked;
        public bool IsDoorChecked
        {
            get => _isDoorChecked;
            set
            { 
                _isDoorChecked = value;
                UpdateAvailability();
                OnAddDoorClicked();
                OnPropertyChanged();
            }
        }
        private bool _isGlassChecked;
        public bool IsGlassChecked
        {
            get => _isGlassChecked;
            set
            {
                _isGlassChecked = value;
                UpdateAvailability();
                OnGlassDoorClicked();
                OnPropertyChanged();
            }
        }

        private bool _enablecheckDoor;
        public bool EnablecheckDoor
        {
            get => _enablecheckDoor;
            set
            {
                _enablecheckDoor = value;
                OnPropertyChanged();
            }
        }
        private bool _isGlassEnabled;
        public bool IsGlassEnabled
        {
            get => _isGlassEnabled;
            set
            {
                _isGlassEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isDoorVisible;
        public bool IsDoorVisible
        {
            get => _isDoorVisible;
            set
            {
                _isDoorVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isGlassVisible;
        public bool IsGlassVisible
        {
            get => _isGlassVisible;
            set
            {
                _isGlassVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isDoorPickerVisible;
        public bool IsDoorPickerVisible
        {
            get => _isDoorPickerVisible;
            set
            {
                _isDoorPickerVisible = value;
                OnPropertyChanged();
            }
        }
        //Selected Item from Picker
        private string _selectedAngleIronColor;
        public string SelectedAngleIronColor
        {
            get => _selectedAngleIronColor;
            set
            {
                if (_selectedAngleIronColor != value)
                {
                    _selectedAngleIronColor = value;
                    UpdateAvailability();
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedLockerColorItem;
        public string SelectedLockerColorItem
        {
            get => _selectedLockerColorItem;
            set
            {
                if (_selectedLockerColorItem != value)
                {
                    _selectedLockerColorItem = value;
                    UpdateAvailability();
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedHeightItem;
        public string SelectedHeightItem
        {
            get => _selectedHeightItem;
            set
            {
                if (_selectedHeightItem != value)
                {
                    _selectedHeightItem = value;
                    UpdateAvailability();
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedDoorColorItem;
        public string SelectedDoorColorItem
        {
            get => _selectedDoorColorItem;
            set
            {
                if (_selectedDoorColorItem != value)
                {
                    _selectedDoorColorItem = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _selectedDoorMaterialItem;
        public string SelectedDoorMaterialItem
        {
            get => _selectedDoorMaterialItem;
            set
            {
                if (_selectedDoorMaterialItem != value)
                {

                    _selectedDoorMaterialItem = value;
                    ItemSourceDoorPicker.Clear(); 
                    SelectedDoorColorItem = null;//On remets le picker color à 0 quand on change de materiel

                    UpdateAvailability();


                    OnPropertyChanged();

                    UpdateAvailability();



                }
            }
        }

        private string _selectedDepthItem;
        public string SelectedDepthItem
        {
            get => _selectedDepthItem;
            set
            {
                if (_selectedDepthItem != value)
                {
                    _selectedDepthItem = value;
                    OnPropertyChanged();
                    UpdateAvailability();
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedWidthItem;
        public string SelectedWidthItem
        {
            get => _selectedWidthItem;
            set
            {
                if (_selectedWidthItem != value)
                {
                    _selectedWidthItem = value;
                    UpdateAvailability();
                    OnPropertyChanged();   
                }
            }
        }

        //Source Item for picker
        private List<string> _itemSourceAngleIronColor;
        public List<string> ItemSourceAngleIronColor
        {
            get => _itemSourceAngleIronColor;
            set
            {
                if (_itemSourceAngleIronColor != value)
                {
                    _itemSourceAngleIronColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> _itemSourceDoorPicker;
        public List<string> ItemSourceDoorPicker
        {
            get => _itemSourceDoorPicker;
            set
            {
                if (_itemSourceDoorPicker != value)
                {
                    _itemSourceDoorPicker = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> _itemSourceDoorPickerMaterial;
        public List<string> ItemSourceDoorPickerMaterial
        {
            get => _itemSourceDoorPickerMaterial;
            set
            {
                if (_itemSourceDoorPickerMaterial != value)
                {
                    _itemSourceDoorPickerMaterial = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> _itemSourceLockerHeight;
        public List<string> ItemSourceLockerHeight
        {
            get => _itemSourceLockerHeight;
            set
            {
                if (_itemSourceLockerHeight != value)
                {
                    _itemSourceLockerHeight = value;
                    OnPropertyChanged();
                }
            }
        }
        private List<string> _itemSourceLockerColor;
        public List<string> ItemSourceLockerColor
        {
            get => _itemSourceLockerColor;
            set
            {   
                if (_itemSourceLockerColor != value)
                {
                    _itemSourceLockerColor = value;
                    OnPropertyChanged();
                }
            }
        }
        private List<string> _itemSourceLockerDepth;
        public List<string> ItemSourceLockerDepth
        {
            get => _itemSourceLockerDepth;
            set
            {
                if ( _itemSourceLockerDepth != value)
                {
                    _itemSourceLockerDepth = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<string> _itemSourceLockerWidth;
        public List<string> ItemSourceLockerWidth
        {
            get => _itemSourceLockerWidth;
            set
            {
                if ( _itemSourceLockerWidth != value)
                {
                    _itemSourceLockerWidth = value;
                    OnPropertyChanged();
                }
            }
        }


        private float _price;
        public float Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        private float _width;
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        private float _length;
        public float Length
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        private float _totalSize;
        public float TotalSize
        {
            get => _totalSize;
            set
            {
                _totalSize = value;
                OnPropertyChanged();
            }
        }
        private void CalculateTotalSize()
        {
            TotalSize = Lockers.Sum(locker => locker.Height);
        }

        private float _totalPrice;
        public float TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }
        private void CalculateTotalPrice()
        {
            TotalPrice = Lockers.Sum(locker => locker.Price);
        }




        //Setup Visible or Invisible Door selectors when Door Checkbox is checked or unchecked
        private void OnAddDoorClicked()
        {
            if (IsDoorChecked)
            {
                if (IsGlassChecked) { IsGlassVisible = true; selectColorEnabler = false; }
                else {IsGlassVisible = true; selectColorEnabler = true; }
            }
            else
            {
                selectColorEnabler = false;
                IsGlassVisible = false;
                IsGlassChecked = false;
                SelectedDoorColorItem = null;
            }
            ShowColorPicker();
            Debug.WriteLine("OnAddDoorClicked");
        }

        //if Glass checked -> Color picker not visible && unchecked
        //if Glass not checked -> Color picker is available
        private void OnGlassDoorClicked()
        {
            if (IsGlassChecked)
            {
                selectColorEnabler = false;
                SelectedDoorColorItem = "Glass";
            }
            else if (IsDoorChecked)
            {
                selectColorEnabler = true;
            }
            ShowColorPicker();
            Debug.WriteLine("OnGlassDoorClicked");
        }

        //show or hide door color picker
        private void ShowColorPicker()
        {
            if (selectColorEnabler)
            {
                IsDoorPickerVisible = true;
            }
            else
            {
                IsDoorPickerVisible = false;
            }
        }

        //Interlink between every parameters
        //Update ItemSourcePicker Lists to make sure they match the possibility of the catalog
        private async void UpdateAvailability()
        {
            Debug.WriteLine("UpdateAvailability Begin");

            Catalog c = new Catalog(new DatabaseCatalog("storekeeper", "storekeeper"));

            Dictionary<string, object> requestDict = new Dictionary<string, object>()
    {
        {"Width", _selectedWidthItem},
        {"Depth", _selectedDepthItem},
        {"AngleIronColor", _selectedAngleIronColor },
        {"Color", _selectedLockerColorItem },
        {"Height", _selectedHeightItem},
        {"Door", _isDoorChecked },
        {"DoorColor", _selectedDoorColorItem},
        {"DoorMaterial", _selectedDoorMaterialItem }
    };

            var selectedValues = new Dictionary<string, object> {
                                { "Width", requestDict["Width"] }, { "Depth", requestDict["Depth"] },
                                { "Panel_color", requestDict["Color"] }, { "Height", requestDict["Height"] },
                                { "Door", requestDict["Door"] }, { "Door_color", requestDict["DoorColor"] },
                                {"Angle_color", requestDict["AngleIronColor"] }, {"Door_material", requestDict["DoorMaterial"]}};

            var data = await c.GetValues(selectedValues);

            data.TryGetValue("Height", out List<object> heightList);
            if (SelectedLockerColorItem == null) { ItemSourceLockerColor = data["Panel_color"].ConvertAll(obj => obj.ToString()); }
            if (SelectedDepthItem == null) { ItemSourceLockerDepth = data["Depth"].ConvertAll(obj => obj.ToString()); }
            if (SelectedHeightItem == null) { ItemSourceLockerHeight = data["Height"].ConvertAll(obj => obj.ToString()); }
            if (SelectedWidthItem == null) { ItemSourceLockerWidth = data["Width"].ConvertAll(obj => obj.ToString()); }
            if (SelectedDoorColorItem == null) { ItemSourceDoorPicker = data["Door_color"].ConvertAll(obj => obj.ToString()); }
            if (SelectedAngleIronColor == null) { ItemSourceAngleIronColor = data["Angle_color"].ConvertAll(obj => obj.ToString()); }
            if (SelectedDoorMaterialItem == null) { ItemSourceDoorPickerMaterial = data["Door_material"].ConvertAll(obj => obj.ToString()); }


            if (ItemSourceDoorPicker.Count < 0)
            {
                EnablecheckDoor = false;
                Debug.WriteLine("availableDoor.Count < 0");
            }
            else
            {
                EnablecheckDoor = true;
                if (ItemSourceDoorPicker.Contains("Glass"))
                {
                    IsGlassEnabled = true;
                    while (ItemSourceDoorPicker.Contains("Glass"))
                    {
                        ItemSourceDoorPicker.Remove("Glass");
                    }
                }
                else
                {
                    IsGlassEnabled = false;
                }
                Debug.WriteLine("availableDoor.Count > 0");
            }
            Debug.WriteLine("UpdateAvailability End");
        }


        //Reset all the checkboxes and pickers
        private void ResetLocker()
        {
            Debug.WriteLine("ResetLocker");
            IsGlassVisible = false; IsGlassChecked = false; IsGlassEnabled = true;

            EnablecheckDoor = true; IsDoorChecked = false; IsDoorPickerVisible = false;

            SelectedDepthItem = null; SelectedAngleIronColor = null; SelectedDoorColorItem = null;
            SelectedHeightItem = null; SelectedWidthItem = null; SelectedLockerColorItem = null;
        }
        private void ExecuteOnResetLockerButtonClicked()
        {
            Debug.WriteLine("ExecuteOnResetLockerButtonClicked");
            ResetLocker();
        }

        private LockerViewModel _selectedEditLocker;
        public LockerViewModel SelectedEditLocker
        {
            get => _selectedEditLocker;
            set
            {
                _selectedEditLocker = value;
                OnPropertyChanged();
            }
        }


        //Not Used, keep till not substitued
        private void ExecuteOnAddLockerButtonClicked()
        {
            Debug.WriteLine("ExecuteOnAddLockerButtonClicked");
            Door newDoor = null;
            //Verfify every parameters of the locker are set
            if (_selectedWidthItem != null && _selectedDepthItem != null && _selectedLockerColorItem != null && _selectedHeightItem != null)
            {
                if (_isDoorChecked) //if door asked
                {
                    if (_isGlassChecked) //if door material must be "glass"
                    {
                        newDoor = new Door(null, "glass", Convert.ToInt32(_selectedWidthItem), Convert.ToInt32(_selectedHeightItem));
                    }
                    else if (_selectedDoorColorItem != null) //if door color is picked -> create a wooden door with selected color
                    {
                        newDoor = new Door(_selectedDoorColorItem,"wood", Convert.ToInt32(_selectedWidthItem), Convert.ToInt32(_selectedHeightItem));
                    }
                }
                //Locker is created with all selected parameters & newDoor which = null if no door option were selected
                Locker newLocker = new Locker(Convert.ToInt32(_selectedHeightItem), Convert.ToInt32(_selectedDepthItem), Convert.ToInt32(_selectedWidthItem), _selectedLockerColorItem, newDoor, _price);
            }
        }
    }
}

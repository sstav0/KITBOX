using System.Collections.ObjectModel;
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

        DatabaseCatalog databaseCatalog = new DatabaseCatalog("customer","customer");


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
        private int _selectedHeightItem;
        public int SelectedHeightItem
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
                    UpdateAvailability();
                    OnPropertyChanged();
                }
            }
        }
        private int _selectedDepthItem;
        public int SelectedDepthItem
        {
            get => _selectedDepthItem;
            set
            {
                if (_selectedDepthItem != value)
                {
                    _selectedDepthItem = value;
                    UpdateAvailability();
                    OnPropertyChanged();
                }
            }
        }
        private int _selectedWidthItem;
        public int SelectedWidthItem
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
                _itemSourceAngleIronColor = value;
            }
        }

        private List<string> _itemSourceDoorPicker;
        public List<string> ItemSourceDoorPicker
        {
            get => _itemSourceDoorPicker;
            set
            {
                _itemSourceDoorPicker = value;
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
        private void UpdateAvailability()
        {
            Debug.WriteLine("UpdateAvailability Begin");

            Dictionary<string, object> requestDict = new Dictionary<string, object>()
            {
                {"Width", _selectedWidthItem},
                {"Depth", _selectedDepthItem},
                {"AngleIronColor", _selectedAngleIronColor },
                {"Color", _selectedLockerColorItem },
                {"Height", _selectedHeightItem },
                {"DoorColor", _selectedDoorColorItem}
            };

            //List<Dictionary<string, object>> data = databaseCatalog.GetData(requestDict);

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>
            {new Dictionary<string, object>{ { "Color", "Brown" },{ "Depth", 12}, { "Width", 15}, { "Height", 28}, { "DoorColor", "Green"}, { "AngleIronColor", "RED" } },
             new Dictionary<string, object>{ { "Color", "GREY" }, { "Depth", 32 }, { "Width", 32 }, { "Height", 32 }, { "DoorColor", "Glass" },{"AngleIronColor", "BLUE" } } };
 
            ItemSourceLockerColor = data.Select(d => d["Color"].ToString()).ToList();
            ItemSourceLockerDepth = data.Select(d => d["Depth"].ToString()).ToList();
            ItemSourceLockerHeight = data.Select(d => d["Height"].ToString()).ToList();
            ItemSourceLockerWidth = data.Select(d => d["Width"].ToString()).ToList();
            ItemSourceDoorPicker = data.Select(d => d["DoorColor"].ToString()).ToList();
            ItemSourceAngleIronColor = data.Select(d => d["AngleIronColor"].ToString()).ToList();

            //if door color list empty -> Door checkbox is disabled
            if (ItemSourceDoorPicker.Count < 0)
            {
                EnablecheckDoor = false;
                Debug.WriteLine("availableDoor.Count < 0");
            }
            //else -> Remove "Glass" color from the list
            //setup glass checkboc accordingly
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
            IsGlassVisible = false;
            IsGlassChecked = false;
            IsGlassEnabled = true;

            EnablecheckDoor = true;
            IsDoorChecked = false;
            IsDoorPickerVisible = false;

            SelectedDepthItem = 0;
            SelectedAngleIronColor = null;
            SelectedDoorColorItem = null;
            SelectedHeightItem = 0;
            SelectedWidthItem = 0;
            SelectedLockerColorItem = null;
        }
        private void ExecuteOnResetLockerButtonClicked()
        {
            Debug.WriteLine("ExecuteOnResetLockerButtonClicked");
            ResetLocker();
        }

        //Not Used, keep till not substitued
        private void ExecuteOnAddLockerButtonClicked()
        {
            Debug.WriteLine("ExecuteOnAddLockerButtonClicked");
            Door newDoor = null;

            if (_selectedWidthItem != 0 && _selectedDepthItem != 0 && _selectedLockerColorItem != null && _selectedHeightItem != 0)
            {
                if (_isDoorChecked)
                {
                    if (_isGlassChecked)
                    {
                        newDoor = new Door(null, "glass", _selectedWidthItem, _selectedHeightItem);
                    }
                    else if (_selectedDoorColorItem != null)
                    {
                        newDoor = new Door(_selectedDoorColorItem,"wood", _selectedWidthItem, _selectedHeightItem);
                    }
                    else
                    {
                        return;
                    }
                }
                Locker newLocker = new Locker(_selectedHeightItem, _selectedDepthItem, _selectedWidthItem, _selectedLockerColorItem, newDoor, _price);
            }
        }
    }
}

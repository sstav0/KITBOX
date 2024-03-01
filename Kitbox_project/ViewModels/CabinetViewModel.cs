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

        private static Color color1 = new Color(255, 152, 12);
        private static Color color2 = new Color(255, 100, 100);
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

        DatabaseCatalog databaseCatalog = new DatabaseCatalog();


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
            
            IsGlassVisible = false;
            EnablecheckDoor = true;
            IsDoorPickerVisible = false;

            SelectedDepthItem = 0;
            SelectedDoorColorItem = null;
            SelectedHeightItem = 0;
            SelectedWidthItem = 0;
            SelectedLockerColorItem = null;
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

        private bool _isDoorChecked;
        public bool IsDoorChecked
        {
            get => _isDoorChecked;
            set
            { 
                _isDoorChecked = value;
                OnAddDoorClicked();
                UpdateAvailability();
            }
        }
        private bool _isGlassChecked;
        public bool IsGlassChecked
        {
            get => _isGlassChecked;
            set
            {
                _isGlassChecked = value;
                OnGlassDoorClicked();
                UpdateAvailability();
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
            }
            ShowColorPicker();
            Debug.WriteLine("OnAddDoorClicked");
        }

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

        private void UpdateAvailability()
        {
            Dictionary<string, object> requestDict = new Dictionary<string, object>()
            {
                {"Length", _selectedWidthItem},
                {"Depth", _selectedDepthItem},
                {"Color", _selectedLockerColorItem },
                {"Height", _selectedHeightItem },
                {"DoorColor", _selectedDoorColorItem}
            };

            //List<Dictionary<string, object>> data = databaseCatalog.GetData(requestDict);

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>
            {new Dictionary<string, object>{ { "Color", "Brown" },{ "Depth", 12}, { "Length", 15}, { "Height", 28}, { "DoorColor", "Green"} } };

            ItemSourceLockerColor = data.Select(d => d["Color"].ToString()).ToList();
            ItemSourceLockerDepth = data.Select(d => d["Depth"].ToString()).ToList();
            ItemSourceLockerHeight= data.Select(d => d["Height"].ToString()).ToList();
            ItemSourceLockerWidth = data.Select(d => d["Length"].ToString()).ToList();
            ItemSourceDoorPicker = data.Select(d => d["DoorColor"].ToString()).ToList();
            
            if (ItemSourceDoorPicker.Count < 0)
            {
                EnablecheckDoor = false;
                Debug.WriteLine("availableDoor.Count < 0");
            }
            else
            {
                if (ItemSourceDoorPicker.Contains("Glass"))
                {
                    while (ItemSourceDoorPicker.Contains("Glass"))
                    {
                        ItemSourceDoorPicker.Remove("Glass");
                    }
                }
                EnablecheckDoor = true;
                //IsGlassVisible = true;
            }
            Debug.WriteLine("UpdateAvailability !!");
        }


        private void ExecuteOnAddLockerButtonClicked()
        {
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

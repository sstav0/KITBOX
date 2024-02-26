using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kitbox_project.ViewModels;
using System.Diagnostics;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using System.Windows.Input;
//using Microsoft.UI.Xaml.Controls;

namespace Kitbox_project.ViewModels
{
    public class CabinetViewModel : INotifyPropertyChanged
    {
        public ICommand OnAddLockerButtonClicked { get; }

        private static Color color1 = new Color(255, 152, 12);
        private static Color color2 = new Color(255, 100, 100);
        Door door1 = new Door("color1", "wood", 12, 12);
        Door door2 = new Door("color2", "wood", 12, 12);
        private List<Door> doorsTest = new List<Door>();

        private static DatabaseDoor DatabaseDoorObject = new DatabaseDoor();
        private static DatabaseLocker DatabaseLockerObject = new DatabaseLocker();

        //for 
        private List<Door> doors = new List<Door>();
        private List<Locker> lockers = new List<Locker>();

        private List<Door> availableDoor = DatabaseDoorObject.GetList();
        private List<Locker> availableLocker = DatabaseLockerObject.GetList();
        private readonly List<Door> allDoor = DatabaseDoorObject.GetList();
        private readonly  List<Locker> allLocker = DatabaseLockerObject.GetList();

        private bool selectColorEnabler = true;
        
            
        public CabinetViewModel()
        {
            Lockers = new ObservableCollection<LockerViewModel>();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalPrice();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalSize();
            OnAddLockerButtonClicked = new Command(ExecuteOnAddLockerButtonClicked);
            
            IsGlassVisible = false;
            IsDoorVisible = true;
            IsDoorPickerVisible = false;
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
                OnGlassDoorClicked();
                UpdateAvailability();
                OnPropertyChanged();
            }
        }

        private bool _uncheckDoor = true;
        public bool UncheckDoor
        {
            get => _uncheckDoor;
            set
            {
                _uncheckDoor = value;
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
                    OnPropertyChanged();
                    UpdateAvailability();
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
                    OnPropertyChanged();
                    UpdateAvailability();
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
                    UpdateAvailability();
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
                    OnPropertyChanged();
                    UpdateAvailability();
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
                    OnPropertyChanged();
                    UpdateAvailability();
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
                OnPropertyChanged();
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
   

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<string> LoadDoors()
        {
            List<string> doors = new List<string>();
            foreach (Door door in availableDoor)
            {
                doors.Add(door.GetColor().ToString());
            }

            return doors;
        }
  
        private void OnAddDoorClicked()
        {
            if (IsDoorChecked)
            {
                if (IsGlassChecked) { IsGlassVisible = true; selectColorEnabler = false; }
                else { IsGlassVisible = true; selectColorEnabler = true; }
            }
            else
            {
                selectColorEnabler = false;
                IsGlassVisible = false;
                Debug.WriteLine("false");
            }
            ShowColorPicker();
        }

        private void OnGlassDoorClicked()
        {
            if (IsGlassChecked)
            {
                selectColorEnabler = false;
                Debug.WriteLine("false");
            }
            else if (IsDoorChecked)
            {
                selectColorEnabler = true;
            }
            ShowColorPicker();
        }

        private void ShowColorPicker()
        {
            if (selectColorEnabler)
            {
                IsDoorPickerVisible = true;
                ItemSourceDoorPicker = LoadDoors();
            }
            else
            {
                IsDoorPickerVisible = false;
            }
        }
        private void SubUpdateAvailabilityDoorsHeight(Door anyDoor)
        {
            if (anyDoor.GetMaterial() == "glass" && _isGlassChecked)
            {
                doors.Add(anyDoor);
            }
            else if (anyDoor.GetMaterial() == "wood" && _isDoorChecked)
            {
                doors.Add(anyDoor);
            }
            else if (!_isDoorChecked && !IsGlassChecked)
            {
                doors.Add(anyDoor);
            }
        }
        private void SubUpdateAvailabilityDoorsWidth(Door anyDoor)
        {
            if (anyDoor.GetHeight() == _selectedHeightItem && _selectedHeightItem != 0)
            {
                SubUpdateAvailabilityDoorsHeight(anyDoor);
            }
            else if (_selectedHeightItem == 0)
            {
                SubUpdateAvailabilityDoorsHeight(anyDoor);
            }
        }
        private void SubUpdateAvailabilityLockersWidth(Locker anyLocker)
        {
            if (anyLocker.GetDepth() == _selectedDepthItem && _selectedDepthItem != 0)
            {
                SubUpdateAvailabilityLockersDepth(anyLocker);
            }
            else if (_selectedDepthItem == 0)
            {
                SubUpdateAvailabilityLockersDepth(anyLocker);
            }
        }
        private void SubUpdateAvailabilityLockersDepth(Locker anyLocker)
        {
            if (anyLocker.GetColor().ToString() == _selectedLockerColorItem && _selectedLockerColorItem != null)
            {
                SubUpdateAvailabilityLockersColor(anyLocker);
            }
            else if (_selectedDoorColorItem == null)
            {
                SubUpdateAvailabilityLockersColor(anyLocker);
            }
        }
        private void SubUpdateAvailabilityLockersColor(Locker anyLocker)
        {
            if (anyLocker.GetHeight() == _selectedHeightItem && _selectedHeightItem != 0)
            {
                lockers.Add(anyLocker);
            }
            else if (_selectedHeightItem == 0)
            {
                lockers.Add(anyLocker);
            }
        }


        private void UpdateAvailability()
        {

            doors = new List<Door>();
            lockers = new List<Locker>();

            foreach (Door anyDoor in allDoor)
            {
                if (anyDoor.GetWidth() == _selectedWidthItem && _selectedWidthItem != 0)
                {
                    SubUpdateAvailabilityDoorsWidth(anyDoor);
                }

                else if (_selectedWidthItem == 0)
                {
                    SubUpdateAvailabilityDoorsWidth(anyDoor);
                }
            }
            availableDoor = doors;

            UncheckDoor = availableDoor != null && availableDoor.Count > 0;
            _isDoorChecked = availableDoor != null && availableDoor.Count > 0;


            foreach (Locker anyLocker in allLocker)
            {
                if (anyLocker.GetWidth() == _selectedWidthItem && _selectedWidthItem != 0)
                {
                    SubUpdateAvailabilityLockersWidth(anyLocker);
                }
                else if (_selectedWidthItem == 0)
                {
                    SubUpdateAvailabilityLockersWidth(anyLocker);
                }
            }
            availableLocker = lockers;

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

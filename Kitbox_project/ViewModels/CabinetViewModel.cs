using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kitbox_project.ViewModels;
using System.Diagnostics;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
using System.Windows.Input;
using System.Linq;
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
        private static Locker locker2 = new Locker(12, 11, 11, "Emeraude", door2, 65.2);



       

        
        private List<Door> doors = new List<Door>();
        private List<Locker> lockers = new List<Locker>();

        private List<Door> availableDoor = new List<Door>();
        private List<Locker> availableLocker = new List<Locker>();
        private readonly List<Door> allDoor = new List<Door>();
        private readonly  List<Locker> allLocker = new List<Locker>();

        private bool selectColorEnabler = true;


        public CabinetViewModel()
        {

            availableDoor.Add(door1);
            availableDoor.Add(door2);

            availableLocker.Add(locker1);
            availableLocker.Add(locker2);

            Lockers = new ObservableCollection<LockerViewModel>();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalPrice();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalSize();
            OnAddLockerButtonClicked = new Command(ExecuteOnAddLockerButtonClicked);
            
            IsGlassVisible = false;
            EnablecheckDoor = true;
            IsDoorPickerVisible = false;

            ItemSourceLockerColor = LoadLockerStringList("color");
            ItemSourceLockerDepth = LoadLockerStringList("depth");
            ItemSourceLockerHeight = LoadLockerStringList("height");
            ItemSourceLockerWidth = LoadLockerStringList("width");

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
                //UpdateAvailability();
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
                //UpdateAvailability();
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
                    //UpdateAvailability();
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
                    //UpdateAvailability();
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
                    //UpdateAvailability();
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
                    //UpdateAvailability();
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
                    //UpdateAvailability();
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
                List<string> colorDoorList = new List<string>();
                foreach(Door door in availableDoor)
                {
                    if (!colorDoorList.Contains(door.Color))
                    {
                        colorDoorList.Add(door.Color);
                    }
                }
                Debug.Write(colorDoorList.ToString());
                _itemSourceDoorPicker = colorDoorList;
                OnPropertyChanged();
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
  

        List<string> LoadLockerStringList(string param) 
        {
            List<string> itemSourceList = new List<string>();
            itemSourceList.Add("0");
            foreach (Locker objectItem in availableLocker)
            {
                if (param == "color")
                {
                    if (!itemSourceList.Contains(objectItem.Color))
                    {
                        itemSourceList.Add(objectItem.Color);
                    }
                }
                else if (param == "height")
                {
                    if (!itemSourceList.Contains(objectItem.Height.ToString()))
                    {
                        itemSourceList.Add(objectItem.Height.ToString());
                    }
                }
                else if (param == "depth")
                {
                    if (!itemSourceList.Contains(objectItem.Depth.ToString()))
                    {
                        itemSourceList.Add(objectItem.Depth.ToString());
                    }
                }
                else if (param == "width")
                {
                    if (!itemSourceList.Contains(objectItem.Width.ToString()))
                    {
                        itemSourceList.Add(objectItem.Width.ToString());
                    }
                }
            }
            return itemSourceList;
        }
        private List<string> LoadDoorsColor()
        {
            List<string> doors = new List<string>();
            foreach (Door door in availableDoor)
            {
                doors.Add(door.Color.ToString());
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
                ItemSourceDoorPicker = LoadDoorsColor();
            }
            else
            {
                IsDoorPickerVisible = false;
            }
        }
        private void SubUpdateAvailabilityDoorsHeight(Door anyDoor)
        {
            if (anyDoor.Material == "glass" && _isGlassChecked)
            {
                doors.Add(anyDoor);
            }
            else if (anyDoor.Material == "wood" && _isDoorChecked)
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
            if (anyDoor.Height == _selectedHeightItem && _selectedHeightItem != 0)
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
            if (anyLocker.Depth == _selectedDepthItem && _selectedDepthItem != 0)
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
            if (anyLocker.Color.ToString() == _selectedLockerColorItem && _selectedLockerColorItem != null)
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
            if (anyLocker.Height == _selectedHeightItem && _selectedHeightItem != 0)
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
                if (anyDoor.Width == _selectedWidthItem && _selectedWidthItem != 0)
                {
                    SubUpdateAvailabilityDoorsWidth(anyDoor);
                }

                else if (_selectedWidthItem == 0)
                {
                    SubUpdateAvailabilityDoorsWidth(anyDoor);
                }
            }
            availableDoor = doors;
            Debug.WriteLine("Y A DES PORTES ??");
            Debug.WriteLine(availableDoor != null && availableDoor.Count > 0);

            EnablecheckDoor = availableDoor != null && availableDoor.Count > 0;
            IsDoorChecked = availableDoor != null && availableDoor.Count > 0;

            foreach (Locker anyLocker in allLocker)
            {
                if (anyLocker.Width == _selectedWidthItem && _selectedWidthItem != 0)
                {
                    SubUpdateAvailabilityLockersWidth(anyLocker);
                }
                else if (_selectedWidthItem == 0)
                {
                    SubUpdateAvailabilityLockersWidth(anyLocker);
                }
            }
            availableLocker = lockers;

            ItemSourceLockerColor = LoadLockerStringList("color");
            ItemSourceLockerDepth = LoadLockerStringList("depth");
            ItemSourceLockerHeight = LoadLockerStringList("height");
            ItemSourceLockerWidth = LoadLockerStringList("width");

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

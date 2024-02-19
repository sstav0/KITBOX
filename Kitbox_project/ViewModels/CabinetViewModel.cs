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
        private List<Door> doorList = new List<Door>();
        private static Color color1 = new Color(255, 152, 12);
        private static Color color2 = new Color(255, 100, 100);
        Door door1 = new Door(color1, "wood");
        Door door2 = new Door(color2, "wood");
        private List<Door> doorsTest = new List<Door>();

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
                }
            }
        }
        private string _selectedLengthItem;
        public string SelectedLengthItem
        {
            get => _selectedLengthItem;
            set
            {
                if (_selectedLengthItem != value)
                {
                    _selectedLengthItem = value;
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

        private DatabaseDoor DatabaseDoorObject = new DatabaseDoor();
        private DatabaseLocker DatabaseLockerObject = new DatabaseLocker();
        private bool selectColorEnabler = true;


        private List<string> LoadDoors()//int height, int width)
        {
            List<string> doors = new List<string>();
            doorList = DatabaseDoorObject.GetList();
            foreach (Door door in doorList)
            {
                doors.Add(door.GetColor().ToString());
            }

            return doors;
        }
  
        private void OnAddDoorClicked()
        {
            if (IsDoorChecked && !IsGlassChecked)
            {
                selectColorEnabler = true;
                IsGlassVisible = true;
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
        
        private void ExecuteOnAddLockerButtonClicked()
        {
            if (IsDoorChecked)
            {
                if (IsGlassChecked)
                {
                    Door newDoor = new Door(null, "glass");
                }
                else
                {
                    List<Door> doorList = DatabaseDoorObject.GetList();
                    //Door newDoor = doorList[SelectedDoorColorItem];
                    //Color newColor = new Color(SelectedDoorColorItem);
                }
                
            }
            else
            // /!\ empecher de sélectionner options impossibles ex : Couleur non disponible dans une certaine dimension
            {
                List<Locker> lockers = DatabaseLockerObject.GetList();
               // Locker newLocker = lockers[3];
            }
        }
    }

}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kitbox_project.ViewModels;
using System.Diagnostics;
using Kitbox_project.DataBase;
using Kitbox_project.Models;
//using Microsoft.UI.Xaml.Controls;

namespace Kitbox_project.ViewModels
{
    public class CabinetViewModel : INotifyPropertyChanged
    {
        public CabinetViewModel()
        {
            Lockers = new ObservableCollection<LockerViewModel>();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalPrice();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalSize();
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
 
        private List<string> _itemSourceDoorPicker;
        public  List<string> ItemSourceDoorPicker
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
            TotalPrice = Lockers.Sum(locker =>locker.Price);
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DatabaseDoor DatabaseDoorObject = new DatabaseDoor();
        private bool selectColorEnabler = true;


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
    }

}

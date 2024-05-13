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

        private bool selectColorEnabler = true;

        private List<string> oldItemSourceLockerColor = ["empty"];
        private List<string> oldItemSourceLockerHeight = ["empty"];
        private List<string> oldItemSourceLockerDepth = ["empty"];
        private List<string> oldItemSourceLockerWidth= ["empty"];
        private List<string> oldItemSourceAngleIronColor = ["empty"];
        private List<string> oldItemSourceDoorPickerMaterial = ["empty"];
        private List<string> oldItemSourceDoorPicker = ["empty"];

        public List<Dictionary<string, int>> registeredPartsRefQuantityList = new List<Dictionary<string, int>>();

        private List<Locker> availableLocker = new List<Locker>();
        public Dictionary<string,object> selectedValues = new Dictionary<string,object>();

        private readonly DatabaseCatalog databaseCatalog = new DatabaseCatalog("storekeeper", "storekeeper");

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
                    Debug.WriteLine("AngleIron Change");
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
                if (_itemSourceLockerDepth != value)
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
                if (_itemSourceLockerWidth != value)
                {
                    _itemSourceLockerWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        public CabinetViewModel(List<Dictionary<string, int>> registeredPartsRefQuantity = null)
        {
            Lockers = new ObservableCollection<LockerViewModel>();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalPrice();
            Lockers.CollectionChanged += (sender, e) => CalculateTotalSize();
            OnResetLockerButtonClicked = new Command(ExecuteOnResetLockerButtonClicked);

            if (registeredPartsRefQuantity != null) { this.registeredPartsRefQuantityList = registeredPartsRefQuantity; Debug.WriteLine("ViewModel Dict Passed"); }

            ResetLocker();

            UpdateAvailability();
            Debug.WriteLine("1");
        }

        private ObservableCollection<LockerViewModel> _lockers;
        public ObservableCollection<LockerViewModel> Lockers
        {
            get => _lockers;
            set
            {
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
                OnAddDoorClicked();
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

        private string _selectedDoorMaterialItem;
        public string SelectedDoorMaterialItem
        {
            get => _selectedDoorMaterialItem;
            set
            {
                if (_selectedDoorMaterialItem != value)
                {
                    _selectedDoorMaterialItem = value;
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

        private string _selectedWidthItem;
        public string SelectedWidthItem
        {
            get => _selectedWidthItem;
            set
            {
                if (_selectedWidthItem != value)
                {
                    _selectedWidthItem = value;
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
                selectColorEnabler = true;
            }
            else
            {
                selectColorEnabler = false;
                SelectedDoorColorItem = null;
                SelectedDoorMaterialItem = null;
            }
            ShowColorPicker();
        }

        ///show or hide door color picker
        private void ShowColorPicker()
        {    
            IsDoorPickerVisible = selectColorEnabler;
        }

        /// <summary>
        /// Updates the list of selectable items for a given picker based on currently selected values in other pickers.
        /// </summary>
        /// <param name="param">The key of the picker to update.</param>
        /// <remarks>
        /// This method is called when the dropdown menu of a picker is opened. It retrieves the currently selected values in other pickers,
        /// then updates the list of selectable items for the specified picker based on these values. It also ensures that the currently
        /// selected item in the picker is preserved when updating the list of selectable items.
        /// </remarks>
        public async void UpdatePickerList(string param)//, string selectedItem)
        {
            List<string> newValue = new List<string>(); 
            selectedValues = new Dictionary<string, object> {
                                { "Width", _selectedWidthItem }, { "Depth", _selectedDepthItem },
                                { "Panel_color", _selectedLockerColorItem }, { "Height", _selectedHeightItem },
                                { "Door", _isDoorChecked }, { "Door_color", _selectedDoorColorItem },
                                {"Angle_color", _selectedAngleIronColor }, {"Door_material", _selectedDoorMaterialItem}};

            //The method is called when the picker is open so we can ignore the picker for the catalog search and set it to 'null'
            var savedSelectedValue = selectedValues[param];
            selectedValues[param] = null;

            Catalog c = new Catalog(databaseCatalog, selectedValues);

            var data = await c.GetPickerValues();

            //Re-give the ignored value of the selectedPickerItem to selectedValues Dict (for other applications)
            selectedValues[param] = savedSelectedValue;

            if (data.Keys.Contains(param))
            {
                newValue = data[param].ConvertAll(obj => obj.ToString());
            }

            if (newValue != null) {
                //Check the aimed picker and get the possible items for the picker  
                if (param == "Panel_color" && !newValue.SequenceEqual(oldItemSourceLockerColor))            { ItemSourceLockerColor = data["Panel_color"].ConvertAll(obj => obj.ToString());            oldItemSourceLockerColor = ItemSourceLockerColor; }
                if (param == "Depth" && !newValue.SequenceEqual(oldItemSourceLockerDepth))                  { ItemSourceLockerDepth = data["Depth"].ConvertAll(obj => obj.ToString());                  oldItemSourceLockerDepth = ItemSourceLockerDepth; }
                if (param == "Height" && !newValue.SequenceEqual(oldItemSourceLockerHeight))                { ItemSourceLockerHeight = data["Height"].ConvertAll(obj => obj.ToString());                oldItemSourceLockerHeight = ItemSourceLockerHeight; }
                if (param == "Width" && !newValue.SequenceEqual(oldItemSourceLockerWidth))                  { ItemSourceLockerWidth = data["Width"].ConvertAll(obj => obj.ToString());                  oldItemSourceLockerWidth = ItemSourceLockerWidth; }
                if (param == "Door_color" && !newValue.SequenceEqual(oldItemSourceDoorPicker))              { ItemSourceDoorPicker = data["Door_color"].ConvertAll(obj => obj.ToString());              oldItemSourceDoorPicker = ItemSourceDoorPicker; }
                if (param == "Angle_color" && !newValue.SequenceEqual(oldItemSourceAngleIronColor))         { ItemSourceAngleIronColor = data["Angle_color"].ConvertAll(obj => obj.ToString());         oldItemSourceAngleIronColor = ItemSourceAngleIronColor; }
                if (param == "Door_material" && !newValue.SequenceEqual(oldItemSourceDoorPickerMaterial))   { ItemSourceDoorPickerMaterial = data["Door_material"].ConvertAll(obj => obj.ToString());   oldItemSourceDoorPickerMaterial = ItemSourceDoorPickerMaterial; }
            }

            if (ItemSourceDoorPicker == null || ItemSourceDoorPicker.Count < 0)
            {
                EnablecheckDoor = false;
            }
            else
            {
                EnablecheckDoor = true;
            }
        }

        /// <summary>
        /// Updates the availability of selectable items for all pickers based on currently selected values.
        /// </summary>
        /// <remarks>
        /// This method is called to update the availability of selectable items for all pickers. It internally invokes
        /// the <see cref="UpdatePickerList"/> method for each picker to ensure that the list of selectable items
        /// corresponds to the currently selected values in other pickers.
        /// </remarks>
        private void UpdateAvailability()
        {
            UpdatePickerList("Depth");
            UpdatePickerList("Panel_color");
            UpdatePickerList("Height");
            UpdatePickerList("Width");
            UpdatePickerList("Door_color");
            UpdatePickerList("Angle_color");
            UpdatePickerList("Door_material");
        }

        public async Task<string> NotePartsAvailabilityAsync(Locker lockerToAdd, int modifyIndex = -1)
        {
            Debug.WriteLine("--- NotePartsAvailability ---");
            string message = "Some parts are currently not in our stock for this locker";

            var partAvailabilityResult = await lockerToAdd.ArePartsAvailable(registeredPartsRefQuantityList);
            if(modifyIndex < 0) { registeredPartsRefQuantityList.Add(partAvailabilityResult.Item2); }
            else if (modifyIndex >= 0)
            {
                registeredPartsRefQuantityList[registeredPartsRefQuantityList.Count - AvailableLockers.Count - 1 + modifyIndex] = partAvailabilityResult.Item2;
            }
            
            if(partAvailabilityResult.Item1 == true)
            {
                message = "";
            }

            return message;
        }

        //Reset all the checkboxes and pickers
        private void ResetLocker()
        {
            Debug.WriteLine("ResetLocker");
            EnablecheckDoor = true; IsDoorChecked = false; IsDoorPickerVisible = false;

            SelectedDepthItem = null; SelectedAngleIronColor = null; SelectedDoorColorItem = null;
            SelectedHeightItem = null; SelectedWidthItem = null; SelectedLockerColorItem = null;
            if (AvailableLockers != null)
            {
                registeredPartsRefQuantityList.RemoveRange(registeredPartsRefQuantityList.Count - AvailableLockers.Count, AvailableLockers.Count);
                AvailableLockers.Clear();
            }
            OnPropertyChanged(nameof(AvailableLockers));
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
    }
}

// LockerViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kitbox_project.Models;

namespace Kitbox_project.ViewModels
{
    public class LockerViewModel : INotifyPropertyChanged
    {

        private int _lockerID;
        public int LockerID
        {
            get => _lockerID;
            set 
            {
                _lockerID = value;
                OnPropertyChanged();
            }
        }

        //Locker contained in the LockerViewModel
        private Locker _locker;
        public Locker Locker
        {
            get => _locker;
            set
            {
                _locker = value;
                OnPropertyChanged();
            }
        }


        // string to notify the missing parts for a Locker
        private string _notePartsAvailability;
        public string NotePartsAvailability
        {
            get => _notePartsAvailability;
            set
            {
                _notePartsAvailability = value;
                OnPropertyChanged();
            }
        }


        private float _height;
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                if (Locker != null) { Locker.Height = Convert.ToInt32(_height); }
                OnPropertyChanged();
            }
        }



        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                if (Locker != null) { Locker.Color = _color; }
                OnPropertyChanged();
            }
        }

        private Door _door;
        public Door Door
        {
            get => _door;
            set
            {
                _door = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DoorColor)); // Notify that DoorColor property has changed

            }
        }

        public string DoorColor => _door?.Color; // Return door color if the door object is not null


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


        private int _lockerIndex;
        public int LockerIndex
        {
            get => _lockerIndex;
            set => _lockerIndex = value;
        }

        public void CalculateIndex()
        {
            LockerIndex = LockerID + 1;
        }

        public override string ToString()
        {
            return $"Height: {Height}, Color: {Color}, Door: {Door}, Price {Price}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

// LockerViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Kitbox_project.Models;

namespace Kitbox_project.ViewModels
{
    public class LockerViewModel : INotifyPropertyChanged
    {
        private float _height;
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
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
                OnPropertyChanged();
            }
        }

        private bool _door;
        public bool Door
        {
            get => _door;
            set
            {
                _door = value;
                OnPropertyChanged();
            }
        }
        public override string ToString()
        {
            return $"Height: {Height}, Color: {Color}, Door: {(Door ? "Yes" : "No")}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

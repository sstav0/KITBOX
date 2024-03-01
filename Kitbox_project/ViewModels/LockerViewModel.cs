﻿// LockerViewModel.cs
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
            set => _lockerID = value;
        }
            


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

        private Door _door;
        public Door Door
        {
            get => _door;
            set
            {
                _door = value;
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

        //Pour voir les lockers. Il faudra peut-être changer ça une fois la DB faite
        public override string ToString()
        {
            return $"Height: {Height}, Color: {Color}, Door: {Door}, Price {Price}";
        }

        public event PropertyChangedEventHandler PropertyChanged; //utilité ?

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using Kitbox_project.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Cabinet
    {
        private List<Locker> _lockers;
        private double _price;
        private int _depth;
        private int _length;
        private int _quantity; //necessary ? 
        private int _height;
        private int _cabinetID;

        public Cabinet(List<Locker> lockers, int depth, int length, int quantity)
        {
            this._lockers = lockers;
            this._depth = depth;
            this._length = length;
            this._quantity = quantity;
        }

        public int Height
        {
            get
            {
                this._height = 0;
                foreach (Locker locker in this._lockers)
                {
                    this._height += locker.Height;
                }
                return this._height;
            }
        }

        public double Price
        {
            get
            {
                this._price = 0; //reset the price
                foreach (Locker locker in this._lockers)
                {
                    this._price += locker.Price;
                }
                return this._price;
            }
        }

        public int GetLockerCount() 
        {
            return this._lockers.Count; 
        }

        public ObservableCollection<Locker> GetObservableLockers()
        {
            ObservableCollection<Locker> i = new ObservableCollection<Locker>();
            foreach(Locker locker in this._lockers)
            {
                i.Add(locker);
            }
            return i;
        }

        public override string ToString()
        {
            this._price = Price; //recalculate the price

            string i = string.Join(",", this._lockers);

            return String.Format("{0} {1} {2} {3} {4} {5} {6}", i, this._price.ToString(), this._depth.ToString(), this._price.ToString(), this._length.ToString(), this._quantity.ToString());
        }
        public string ToCart()
        {
            this._price = Price;

            string i = string.Join(",", this._lockers);

            i += $"{this._price.ToString()}, ";
            i += $"{this._depth.ToString()}, ";
            i += $"{this._length.ToString()}, ";
            i += $"{GetLockerCount().ToString()}, ";
            i += $"{this._quantity.ToString()}";
            return i;
        }

        public int Depth
        {
            get => _depth;
            set => _depth = value;
        }

        public int Length
        {
            get => _length;
            set => _length = value;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }
            
        public int CabinetID
        {
            get => _cabinetID;
        }

        public void AddLocker(Locker locker)
        {
            this._lockers.Add(locker);
        }

        public void AddLockerWithIndex(Locker locker, int index)
        {
            this._lockers.Insert(index, locker);
        }

        public void RemoveLocker(int index) 
        {
            this._lockers.RemoveAt(index);
        }
    }
}

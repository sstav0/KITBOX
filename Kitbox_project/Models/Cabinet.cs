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
        private List<Locker> lockers { get; set; } = new List<Locker>();
        private double price { get; set; }
        private int depth { get; set; }
        private int length { get; set; }
        private int quantity { get; set; }

        public Cabinet(List<Locker> lockers, int depth, int length, int quantity)
        {
            this.lockers = lockers;
            this.price = GetPrice();
            this.depth = depth;
            this.length = length;
            this.quantity = quantity;
        }

        public int GetLockerCount() 
        {
            return this.lockers.Count(); 
        }

        public float GetHeight()
        {
            int i = 0;
            foreach (Locker locker in this.lockers) 
            {
                i += locker.GetHeight();
            }
            return i;
        }

        public int GetDepth()
        {
            return this.depth;
        }

        public int GetLength() 
        {
            return this.length;
        }

        public int GetQuantity()
        {
            return this.quantity;
        }

        public double GetPrice()
        {
            double price = 0;
            foreach(Locker locker in this.lockers)
            {
                price += locker.GetPrice();
            }
            this.price = price;
            return price;
        }

        public ObservableCollection<Locker> GetObservableLockers()
        {
            ObservableCollection<Locker> i = new ObservableCollection<Locker>();
            foreach(Locker locker in this.lockers)
            {
                i.Add(locker);
            }
            return i;
        }

        public override string ToString()
        {
            string i = string.Empty;
            foreach (Locker locker in this.lockers)
            {
                i += $"{locker.ToString()}, ";
            }
            i += $"{GetPrice().ToString()}, ";
            i += $"{this.depth.ToString()}, ";
            i += $"{this.length.ToString()}, ";
            i += $"{this.quantity.ToString()}";
            return i;

        }

        public void SetDepth(int depth)
        {
            this.depth = depth;
        }

        public void SetLength(int length)
        {
            this.length = length;
        }

        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        public void AddLocker(Locker locker)
        {
            this.lockers.Add(locker);
        }

        public void AddLockerWithIndex(Locker locker, int index)
        {
            this.lockers.Insert(index, locker);
        }

        public void RemoveLocker(int index) 
        {
            this.lockers.RemoveAt(index);
        }
    }
}

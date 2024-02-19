using System;
using System.Collections.Generic;
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

        public Cabinet(List<Locker> lockers, double price, int depth, int length, int quantity)
        {
            this.lockers = lockers;
            this.price = price;
            this.depth = depth;
            this.length = length;
            this.quantity = quantity;
        }

        public int GetLockerCount() 
        {
            return lockers.Count; 
        }

        public float GetHeight()
        {
            int i = 0;
            foreach (Locker locker in lockers) 
            {
                i += locker.GetHeight();
            }
            return i;
        }

        public float GetDepth()
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
            foreach(Locker locker in lockers)
            {
                price += locker.GetPrice();
            }
            this.price = price;
            return price;
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
            lockers.Add(locker);
        }

        public void AddLockerWithIndex(Locker locker, int index)
        {
            lockers.Insert(index, locker);
        }

        public void RemoveLocker(int index) 
        {
            lockers.RemoveAt(index);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

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
        private float price { get; set; }
        private int width { get; set; }
        private int length { get; set; }
        private int quantity { get; set; }

        private Cabinet(List<Locker> lockers, float price, int width, int length, int quantity)
        {
            this.lockers = lockers;
            this.price = price;
            this.width = width;
            this.length = length;
            this.quantity = quantity;
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

        public float GetWidth()
        {
            return this.width;
        }

        public int GetLength() 
        {
            return this.length;
        }

        public int GetQuantity()
        {
            return this.quantity;
        }

        public float GetPrice()
        {
            return this.price;
        }

        public void SetWidth(int width)
        {
            this.width = width;
        }

        public void SetLength(int length)
        {
            this.length = length;
        }

        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        public void AddLocker(Locker locker, int index)
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

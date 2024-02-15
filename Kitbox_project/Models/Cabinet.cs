using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Cabinet
    {
        private List<Locker> lockers { get; set; }
        private float price { get; set; }
        private int width { get; set; }
        private float length { get; set; }
        private int quantity { get; set; }

        private Cabinet(List<Locker> lockers, float price, int width, float length, int quantity)
        {
            this.lockers = lockers;
            this.price = price;
            this.width = width;
            this.length = length;
            this.quantity = quantity;
        }

        public float GetHeight()
        {
            return 0;
        }

        public float GetWidth()
        {
            return this.width;
        }

        public void SetWidth(int width) 
        {
            this.width = width;
        }

        public int Quantity()
        {
            return this.quantity;
        }

        public float GetPrice()
        {
            return this.price;
        }

        public void AddLocker(Locker locker, int index)
        {
            lockers.Insert(index, locker);
        }

        public void RemoveLocker(int index) 
        {
            lockers.RemoveAt(index);
        }
    }
}

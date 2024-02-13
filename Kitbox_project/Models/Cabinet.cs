using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Cabinet
    {
        public List<Locker> lockers { get; set; } = new List<Locker>();
        public float price { get; set; }
        public float width { get; set; }
        public float length { get; set; }
        public int quantity { get; set; }

        public Cabinet(List<Locker> lockers, float price, float width, float length, int quantity)
        {
            this.lockers = lockers;
            this.price = price;
            this.width = width;
            this.length = length;
            this.quantity = quantity;
        }
    }
}

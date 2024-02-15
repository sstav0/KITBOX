using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Locker
    {
        private int height { get; set; }
        private string color { get; set; }
        private Door door { get; set; }

        private float price { get; set; }

        private Locker(int height, string color, Door door, float price) 
        {
            this.height = height;
            this.color = color;
            this.door = door;
            this.price = price;
        }

        public int GetHeight()
        {
            return this.height;
        }

        public float GetPrice()
        {
            return this.price;
        }

        public string GetColor()
        {
            return this.color;
        }

        public Door GetDoor() 
        {
            return this.door;
        }

        public void SetDoorColor(string color)
        {
            this.door.SetColor(color);
        }

        public void SetDoorMaterial(string color, string material)
        {
            this.door.SetMaterial(material);
        }

        public void SetHeight(int height)
        {
            this.height = height;
        }

        public void SetColor(string color) 
        {
            this.color = color;
        }

        public void SetPrice(float price)
        {
            this.price = price;
        }
    }
}

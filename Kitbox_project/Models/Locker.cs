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
        private Color color { get; set; }
        private Door door { get; set; }

        private float price { get; set; }

        public Locker(int height, Color color, Door door, float price) 
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

        public Color GetColor()
        {
            return this.color;
        }

        public Door GetDoor() 
        {
            return this.door;
        }

        public void SetDoorColor(Color color)
        {
            this.door.SetColor(color);
        }

        public void SetDoorMaterial(string material)
        {
            this.door.SetMaterial(material);
        }

        public void SetHeight(int height)
        {
            this.height = height;
        }

        public void SetColor(Color color) 
        {
            this.color = color;
        }

        public void SetPrice(float price)
        {
            this.price = price;
        }
    }
}

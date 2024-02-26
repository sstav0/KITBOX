﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Locker
    {
        private int height { get; set; }
        private int width { get; set; }
        private int depth { get; set; }
        private string color { get; set; }
        private Door door { get; set; }

        private double price { get; set; }

        public Locker(int height, int depth, int width, string color, Door door, double price) 
        {
            this.height = height;
            this.width = width;
            this.depth = depth;
            this.color = color;
            this.door = door;
            this.price = price;
        }

        public int GetHeight()
        {
            return this.height;
        }

        public double GetDepth()
        {
            return this.depth;
        }

        public double GetPrice()
        {
            return this.price;
        }

        public string GetColor()
        {
            return this.color;
        }

        public int GetWidth()
        {
            return this.width;
        }

        public Door GetDoor() 
        {
            return this.door;
        }

        public override string ToString()
        {
            string i = string.Empty;
            i += $"{this.height.ToString()}, ";
            i += $"{this.color}, ";
            i += $"{this.door.GetColor().ToString()}, ";
            i += $"{this.door.GetMaterial().ToString()}, ";
            i += $"{this.price.ToString()}";
            return i;
        }

        public void SetDoorColor(string color)
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

        public void SetColor(string color) 
        {
            this.color = color;
        }

        public void SetPrice(double price)
        {
            this.price = price;
        }
    }
}
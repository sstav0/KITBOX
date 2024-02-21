using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Door
    {
        private string color { get; set; }
        private string material { get; set; }
        private int width { get; set; }
        private int height { get; set; }

        public Door(string color, string material, int width, int heigth) 
        {
            this.color = color;
            this.material = material;
            this.width = width; 
            this.height = heigth;   
        }

        public string GetColor()
        { 
            return color; 
        }

        public string GetMaterial()
        {
            return material;
        }

        public int GetWidth()
        {
            return width;
        }
        public int GetHeight()
        {
            return height;
        }

        public void SetColor(string color)
        {
            this.color = color;
        }

        public void SetMaterial(string material)
        {
            this.material = material;
        }
    }
}

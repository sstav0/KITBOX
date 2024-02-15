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

        private Door(string color, string material) 
        {
            this.color = color;
            this.material = material;
        }

        public string GetColor()
        { 
            return color; 
        }

        public string GetMaterial()
        {
            return material;
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

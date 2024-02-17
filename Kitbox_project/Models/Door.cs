using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Door
    {
        private Color color { get; set; }
        private string material { get; set; }

        public Door(Color color, string material) 
        {
            this.color = color;
            this.material = material;
        }

        public Color GetColor()
        { 
            return color; 
        }

        public string GetMaterial()
        {
            return material;
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void SetMaterial(string material)
        {
            this.material = material;
        }
    }
}

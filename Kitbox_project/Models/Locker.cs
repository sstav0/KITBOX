using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    internal class Locker
    {
        public float height { get; set; }
        public string color { get; set; }
        public bool door { get; set; }

        public Locker(float height, string color, bool door) 
        {
            this.height = height;
            this.color = color;
            this.door = door;
        }
    }
}

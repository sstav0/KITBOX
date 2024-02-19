using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    class DoorViewModel
    {
        private string color;
        private string material;

        public DoorViewModel(string color, string material)
        {
            this.color = color;
            this.material = material;
        }

        public string GetColor()
        {
            return this.color;
        }
        public string GetMaterial()
        {
            return this.material;
        }

        public void SetColor(string color)
        {
            this.color = color;
        }

        public void SetMaterial(string material)
        {
            this.material= material;
        }
    }
}

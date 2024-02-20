using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    class LockerViewModelV2
    {
        public int height { get; set; }
        public string color { get; set; }
        public DoorViewModel door { get; set; }
        public string price { get; set; }

        public LockerViewModelV2(int height, string color, DoorViewModel door, string price)
        {
            this.height = height;
            this.color = color;
            this.door = door;
            this.price = price;
        }

        public ObservableCollection<string> GetLockerViewModelV2()
        {
            ObservableCollection<string> i = new ObservableCollection<string>();
            i.Add(this.height.ToString());
            i.Add(this.color);
            i.Add(this.door.GetColor());
            i.Add(this.door.GetMaterial());
            i.Add(this.price);
            return i;
        }
        public string GetLockerViewModelStringV2()
        {
            string i = string.Empty;
            i += $"{this.height.ToString()}, ";
            i += $"{this.color}, ";
            i += $"{this.door.GetColor()}, ";
            i += $"{this.door.GetMaterial()}, ";
            i += $"{this.price}";
            return i;
        }

        public void SetLockerViewModelV2(int height, string color, string doorColor, string doorMaterial, string price)
        {
            this.height = height;
            this.color = color;
            this.door.SetColor(doorColor);
            this.door.SetMaterial(doorMaterial);
            this.price = price;
        }
    }
}

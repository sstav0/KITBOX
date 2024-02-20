using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    class CabinetViewModelV2
    {
        public ObservableCollection<LockerViewModelV2> lockers {  get; set; }
        public string price { get; set; }
        public string depth { get; set; }
        public string length { get; set; }
        public string quantity { get; set; }
        public string nbrLockers { get; set; }
        public string height { get; set; }

        public CabinetViewModelV2(ObservableCollection<LockerViewModelV2> lockers,string price, string depth, string length, string quantity) 
        {
            this.lockers = lockers;
            this.price = price;
            this.depth = depth;
            this.length = length;
            this.quantity = quantity;
            this.nbrLockers = lockers.Count().ToString();
            int i = 0;
            foreach (LockerViewModelV2 locker in lockers)
            {
                i += locker.height;
            }
            this.height = i.ToString();
        }

        public void AddLocker(LockerViewModelV2 locker)
        {
            this.lockers.Add(locker);
        }

        public ObservableCollection<string> GetCabinetViewModelV2()
        {
            ObservableCollection<string> i = new ObservableCollection<string>();
            foreach (LockerViewModelV2 locker in this.lockers)
            {
                i.Add(locker.GetLockerViewModelStringV2());
            }
            i.Add(this.price);
            i.Add(this.depth);
            i.Add(this.length);
            i.Add(this.quantity);
            return i;
        }

        public string GetCabinetViewModelStringV2() 
        {
            string i = string.Empty;
            foreach (LockerViewModelV2 locker in this.lockers)
            {
                i += $"{locker.GetLockerViewModelStringV2()}, ";
            }
            i += $"{this.price}, ";
            i += $"{this.depth}, ";
            i += $"{this.length}, ";
            i += $"{this.quantity}";
            return i;
        }

        public string GetPrice() 
        {
            return this.price;
        }

        public string GetDepth()
        {
            return this.depth;
        }

        public string GetLength()
        {
            return this.length;
        }

        public string GetQuantity()
        {
            return this.quantity;
        }

        public string GetNbrLockers() 
        {
            return this.nbrLockers;
        }

        public string GetHeight()
        {
            return this.height;
        }
    }
}

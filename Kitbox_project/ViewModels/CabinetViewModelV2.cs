using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    class CabinetViewModelV2
    {
        public ObservableCollection<Locker> lockers {  get; set; }
        public string price { get; set; }
        public string depth { get; set; }
        public string length { get; set; }
        public string quantity { get; set; }
        public string nbrLockers { get; set; }
        public string height { get; set; }

        public CabinetViewModelV2(Cabinet cabinet) 
        {
            this.lockers = cabinet.GetObservableLockers();
            this.price = cabinet.Price.ToString();
            this.depth = cabinet.Depth.ToString();
            this.length = cabinet.Length.ToString();
            this.quantity = cabinet.Quantity.ToString();
            this.nbrLockers = cabinet.GetLockerCount().ToString();
            this.height = cabinet.Height.ToString();
        }

        public void AddLocker(Locker locker)
        {
            this.lockers.Add(locker);
        }

        public ObservableCollection<string> GetCabinetViewModelV2()
        {
            ObservableCollection<string> i = new ObservableCollection<string>();
            foreach (Locker locker in this.lockers)
            {
                i.Add(locker.ToString());
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
            foreach (Locker locker in this.lockers)
            {
                i += $"{locker.ToString()}, ";
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

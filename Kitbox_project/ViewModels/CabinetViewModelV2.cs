using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    class CabinetViewModelV2
    {
        private ObservableCollection<LockerViewModelV2> lockers {  get; set; }
        private string price { get; set; }
        private string depth { get; set; }
        private string length { get; set; }
        private string quantity { get; set; }

        public CabinetViewModelV2(ObservableCollection<LockerViewModelV2> lockers, string price, string depth, string length, string quantity) 
        {
            this.lockers = lockers;
            this.price = price;
            this.depth = depth;
            this.length = length;
            this.quantity = quantity;
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
    }
}

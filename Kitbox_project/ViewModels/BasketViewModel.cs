using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.ViewModels
{
    class BasketViewModel
    {
        private ObservableCollection<CabinetViewModelV2> basket { get; set; }

        public BasketViewModel() 
        {
            this.basket = new ObservableCollection<CabinetViewModelV2>();
        }

        public void AddToBasket(CabinetViewModelV2 cabinet)
        {
            this.basket.Add(cabinet);
        }

        public ObservableCollection<string> GetBasket()
        {
            ObservableCollection<string> i = new ObservableCollection<string>();
            foreach (var item in this.basket)
            {
                i.Add(item.GetCabinetViewModelStringV2());
            }
            return i;
        }
    }
}

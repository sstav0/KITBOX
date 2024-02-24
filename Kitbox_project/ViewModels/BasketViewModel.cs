using Kitbox_project.Models;
using Microsoft.Maui.Graphics.Text;
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
                i.Add(item.ToString());
            }
            return i;
        }

        public string GetTotalPrice()
        {
            double i = 0;
            foreach(var item in this.basket)
            {
                i += Convert.ToDouble(item.GetPrice());
            }
            return i.ToString();
        }
    }
}

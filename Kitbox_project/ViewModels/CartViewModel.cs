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
    class CartViewModel
    {
        private ObservableCollection<CabinetViewModelV2> cart { get; set; }

        public CartViewModel() 
        {
            this.cart = new ObservableCollection<CabinetViewModelV2>();
        }

        public void AddToBasket(CabinetViewModelV2 cabinet)
        {
            this.cart.Add(cabinet);
        }

        public ObservableCollection<string> GetBasket()
        {
            ObservableCollection<string> i = new ObservableCollection<string>();
            foreach (var item in this.cart)
            {
                i.Add(item.ToString());
            }
            return i;
        }

        public string GetTotalPrice()
        {
            double i = 0;
            foreach(var item in this.cart)
            {
                i += Convert.ToDouble(item.GetPrice());
            }
            return i.ToString();
        }
    }
}

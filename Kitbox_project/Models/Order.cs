using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Order
    {
        private string status { get; set; }
        private List<Cabinet> basket { get; set; }

        public Order(string status, List<Cabinet> basket)
        {
            this.status = status;
            this.basket = basket;
        }

        public void Confirm()
        {

        }

        public void Change_Status(string status)
        {
            this.status = status;
        }

        public void AddCabinet(Cabinet cabinet)
        {
            basket.Add(cabinet);
        }

        public void RemoveCabinet(int index)
        {
            basket.RemoveAt(index);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

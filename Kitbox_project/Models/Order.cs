using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Order
    {
        public string state { get; set; }
        public List<Cabinet> basket { get; set; }

        public Order(string state, List<Cabinet> basket)
        {
            this.state = state;
            this.basket = basket;
        }

        private void Confirm()
        {

        }

        private void Change_Status()
        {

        }
    }
}

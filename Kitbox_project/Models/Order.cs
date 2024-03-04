using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Order
    {
        private string _status;
        private List<Cabinet> _cart;
        private int _id;
        public string Status { get => _status; set => _status = value; }
        private List<Cabinet> Cart { get => _cart; set=> _cart = value; }
        private int ID { get => _id; set => _id = value; }

        public Order(string status, List<Cabinet> cart)
        {
            this._status = status;
            this._cart = cart;
        }

        public void Confirm()
        {
            //TODO
        }

        public void AddCabinet(Cabinet cabinet)
        {
            this._cart.Add(cabinet);
        }

        public void RemoveCabinet(int index)
        {
            this._cart.RemoveAt(index);
        }

        public override string ToString()
        {
            string i = string.Empty;
            i += $"{this._status}, ";
            i += $"{this._cart.ToString()}"; //Does it work ?
            return i;
        }
    }
}

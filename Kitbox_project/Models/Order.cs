using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Order : INotifyPropertyChanged
    {
        private string _status;
        private List<Cabinet> _cart;

        public Order(string status, List<Cabinet> cart)
        {
            this._status = status;
            this._cart = cart;
        }

        public string Status //to see if useful 
        { 
            get => _status;
            set
            {
                _status = value;            
            }
        }

        public List<Cabinet> Cart 
        { 
            get => _cart;
            set
            {
                _cart = value;
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

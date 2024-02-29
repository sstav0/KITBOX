using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Customer
    {
        private string _firstName;
        private string _lastName;
        private List<Order> _orders;
        private string _email;

        public Customer(string firstName, string lastName, List<Order> orders, string email)
        {
            this._firstName = firstName;
            this._lastName = lastName;
            this._orders = orders;
            this._email = email;
        }

        public void AddOrder(Order order)
        {
            this._orders.Add(order);
        }

        public void RemoveOrder(int index) 
        {
            this._orders.RemoveAt(index);
        }

        public List<Order> Orders
        {
            get => _orders;
            set => _orders = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = value;
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = value;
        }
    }
}

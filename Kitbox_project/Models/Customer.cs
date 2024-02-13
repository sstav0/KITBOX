using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    internal class Customer
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public List<Order> orders { get; set; }
        public string email { get; set; }

        public Customer(string firstname, string lastname, List<Order> orders, string email)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.orders = orders;
            this.email = email;
        }

        private void Add_Order(Order order)
        {

        }

        private void Remove_Order() 
        {

        }
    }
}

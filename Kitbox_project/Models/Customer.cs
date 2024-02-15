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
        private string firstName { get; set; }
        private string lastName { get; set; }
        private List<Order> orders { get; set; }
        private string email { get; set; }

        private Customer(string firstName, string lastName, List<Order> orders, string email)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.orders = orders;
            this.email = email;
        }

        public void Add_Order(Order order)
        {

        }

        public void Remove_Order(int index) 
        {

        }

        public List<Order> Get_Orders() 
        {
            return orders;
        }

        public string Get_Email() 
        {
            return this.email;
        }
        public string Get_FirstName() 
        {
            return this.firstName;
        }
        public string Get_LastName() 
        {
            return this.lastName;
        }
        public void Set_Email(string email)
        {
            this.email = email;
        }

        public void Set_FirstName(string firstName)
        {
            this.firstName = firstName;
        }

        public void Set_LastName(string lastName)
        {
            this.lastName = lastName;
        }
    }
}

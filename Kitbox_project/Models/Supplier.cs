using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    public class Supplier
    {
        private int _id;
        private string _name;
        private string _address;
        private string _city;
        private int _postalCode;
        private string _country;

        public Supplier(int id, string name, string address, string city, int postalCode, string country)
        {
            _id = id;
            _name = name;
            _address = address;
            _city = city;
            _postalCode = postalCode;
            _country = country;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string City
        {
            get => _city;
            set => _city = value;
        }

        public int PostalCode
        {
            get => _postalCode;
            set => _postalCode = value;
        }

        public string Country
        {
            get => _country;
            set => _country = value;
        }
    }
}

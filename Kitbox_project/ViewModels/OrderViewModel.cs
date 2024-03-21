using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Kitbox_project.Models;


namespace Kitbox_project.ViewModels
{
    internal class OrderViewModel : INotifyPropertyChanged
    {
        private Order _order;
        public Order Order { get => _order; set => _order = value;}

        private int _orderID;
        public int OrderID { get => _orderID; set => _orderID = value; }

        private string _orderStatus;
        public string OrderStatus 
        { 
            get => _orderStatus; 
            set 
            { 
                _orderStatus = value; 
                OnPropertyChanged(nameof(OrderStatus)); 
            }
        }

        private string _notifaction;
        public string Notification
        {
            get => _notifaction;
            set
            {
                _notifaction = value;
                OnPropertyChanged(nameof(Notification));
            }
        }

        private bool _orderVisibility;
        public bool OrderVisibility
        {
            get => _orderVisibility;
            set
            {
                _orderVisibility = value;
                OnPropertyChanged(nameof(OrderVisibility));
            }
        }

        private List<string> _listParts;
        public List<string> ListParts { get => _listParts; set => _listParts = value; }

        public OrderViewModel(Order order) 
        {
            this.Order = order;
            //this.OrderID = order.OrderID;
            this.OrderStatus = order.Status;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

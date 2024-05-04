using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models;
public class OrderItem:INotifyPropertyChanged
{
    private int _idOrder;
    private int _idCustomer;
    private int _quantity;
    private string _code;
    private string _date;
    public OrderItem (int idOrder, int idCustomer, int quantity, string code, string date){
        _idOrder = idOrder;
        _idCustomer = idCustomer;
        _quantity = quantity;
        _code = code;
        _date = date;

    }

     public int IdOrder
        {
            get => _idOrder;
            set
            {
                _idOrder = value;
                OnPropertyChanged(nameof(IdOrder));
            }
        }
    public int IdCustomer
        {
            get => _idCustomer;
            set
            {
                _idCustomer = value;
                OnPropertyChanged(nameof(IdCustomer));
            }
        }

    public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }    
    public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }  
    public string Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }  

    public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }            
}

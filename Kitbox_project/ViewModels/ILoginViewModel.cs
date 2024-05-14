

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kitbox_project.ViewModels;

public class ILoginViewModel : INotifyPropertyChanged
{
    public ILoginViewModel()
    { }

    private string _user;

    private bool _isSeller = false;
    private bool _isCustomer = false;
    private bool _isDirector = false;
    private bool _isSecretary = false;
    private bool _isStorekeeper = false;

    public void UpdateUserRights(string user)
    {
        switch (user)
        {
            case "customer":
                IsCustomer = true; 
                break;
            case "director":
                IsDirector = true; 
                break;
            case "storekeeper":
                IsStorekeeper = true; 
                break;
            case "seller":
                IsSeller = true; 
                break;
            case "secretary":
                IsSecretary = true; 
                break;
        }
    }

    public string User
    {
        get => _user;
        set
        {
            _user = value;
            OnPropertyChanged(nameof(User));
        }
    }

    public bool IsDirector
    {
        get => _isDirector;
        set
        {
            if (value != _isDirector)
            {
                _isDirector = value;
                OnPropertyChanged(nameof(IsDirector));
            }
        }
    }
    public bool IsSeller
    {
        get => _isSeller;
        set
        {
            _isSeller = value;
            OnPropertyChanged(nameof(IsSeller));
        }
    }
    public bool IsCustomer
    {
        get => _isCustomer;
        set
        {
            _isCustomer = value;
            OnPropertyChanged(nameof(IsCustomer));
        }
    }
    public bool IsStorekeeper
    {
        get => _isStorekeeper;
        set
        {
            _isStorekeeper = value;
            OnPropertyChanged(nameof(IsStorekeeper));
        }
    }
    public bool IsSecretary
    {
        get => _isSecretary;
        set
        {
            _isSecretary = value;
            OnPropertyChanged(nameof(IsSecretary));
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

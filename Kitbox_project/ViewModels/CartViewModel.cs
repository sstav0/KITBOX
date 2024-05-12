using Kitbox_project.Models;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitbox_project.Views;
using CommunityToolkit.Maui.Views;

namespace Kitbox_project.ViewModels;


public class CartViewModel
{
    private Cabinet _cabinet;
    private readonly Page _page;
    public Cabinet Cabinet
    { get => _cabinet; set => _cabinet = value; }

    private ObservableCollection<Locker> _lockers;
    public ObservableCollection<Locker> Lockers
    { get => _lockers; set => _lockers = value; }

    private string _price;
    public string Price
    { get => _price; set => _price = value; }
    
    private int _depth;
    public int Depth
    { get => _depth; set => _depth = value; }

    private int _length;
    public int Length
    { get => _length; set => _length = value; }
    
    private int _quantity;
    public int Quantity
    { get => _quantity; set => _quantity = value; }
    
    private int _nbrLockers;
    public int NbrLockers
    { get => _nbrLockers; set => _nbrLockers = value; }

    private int _height;
    public int Height
    { get => _height; set => _height = value; }

    private int _cabinetID;
    public int CabinetID
    { get => _cabinetID; set => _cabinetID = value; }

    public CartViewModel(Cabinet cabinet, Page page)
    {
        this._cabinet = cabinet;
        this._lockers = cabinet.GetObservableLockers();
        this._price = $"{cabinet.Price.ToString()} €";
        this._depth = cabinet.Depth;
        this._length = cabinet.Length;
        this._quantity = cabinet.Quantity;
        this._nbrLockers = cabinet.GetLockerCount();
        this._height = cabinet.Height;
        this._cabinetID = cabinet.CabinetID;
        _page = page;
    }

    public void AddLocker(Locker locker)
    {
        this._lockers.Add(locker);
    }

    public string GetCabinetViewModelStringV2()
    {
        string i = string.Empty;
        foreach (Locker locker in this._lockers)
        {
            i += $"{locker.ToString()}, ";
        }
        i += $"{this._price}, ";
        i += $"{this._depth}, ";
        i += $"{this._length}, ";
        i += $"{this._quantity}";
        return i;
    }
}

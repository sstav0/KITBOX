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
    private readonly Page _page;

    private Cabinet _cabinet;
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
        _cabinet = cabinet;
        _lockers = cabinet.GetObservableLockers();
        _price = $"{cabinet.Price} €";
        _depth = cabinet.Depth;
        _length = cabinet.Length;
        _quantity = cabinet.Quantity;
        _nbrLockers = cabinet.GetLockerCount();
        _height = cabinet.Height;
        _cabinetID = cabinet.CabinetID;
        _page = page;
    }

    public void AddLocker(Locker locker)
    {
        _lockers.Add(locker);
    }

    public string GetCabinetViewModelStringV2()
    {
        string i = string.Empty;
        foreach (Locker locker in _lockers)
        {
            i += $"{locker}, ";
        }
        i += $"{_price}, ";
        i += $"{_depth}, ";
        i += $"{_length}, ";
        i += $"{_quantity}";
        return i;
    }
}

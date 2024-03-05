using Kitbox_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitbox_project.Models
{
    /// <summary>Creates a locker with specified dimensions, color, door characteristics, and price.
    /// <list type="bullet">
    /// <item> <description>To get the height of the locker use Locker.Height </description>
    /// </item>
    /// <item> <description>To get the width of the locker use Locker.Width </description> </item>
    /// <item> <description>To get the depth of the locker use Locker.Depth </description> </item>
    /// <item> <description>To get the color of the locker use Locker.Color </description> </item>
    /// <item> <description>To get the door of the locker use Locker.Door </description> </item>
    /// <item> <description>To get the price of the locker use Locker.Price </description> </item>
    /// <item> <description>To set the color of the locker, use <c>Locker.Color = string color</c>.</description> </item>
    /// </list>
    /// </summary>
    public class Locker
    {
        private int _height;
        private int _width;
        private int _depth;
        private string _color;
        private Door _door;
        private double _price;
        private int _lockerID;

        /// <summary>
        /// This constructor creates a locker with specified dimensions, color, door characteristics, and price.
        /// </summary>
        /// <param name="height">Height of the locker</param>
        /// <param name="depth">Depth of the cabinet</param>
        /// <param name="width">Width of the cabinet</param>
        /// <param name="color">Color of the panels</param>
        /// <param name="door">Door object associated with this locker</param>
        /// <param name="price">Calculated price of the locker</param>
        public Locker(int height, int depth, int width, string color, Door door, double price) 
        {
            this._height = height;
            this._width = width;
            this._depth = depth;
            this._color = color;
            this._door = door;
            this._price = price;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4} {5}", _height.ToString(), _width.ToString(), _depth.ToString(), _color, _door.ToString(), this._price.ToString());
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }
        public int Width
        {
            get => _width;
            set => _width = value;
        }
        public int Depth
        {
            get => _depth;
            set => _depth = value;
        }

        public string Color
        {
            get => _color;
            set => _color = value;
        }

        public Door Door
        {
            get => _door;
            set => _door = value;
        }

        public double Price
        {
            get => _price;
            set => _price = value;
        }

        public int LockerID
        {
            get => _lockerID;
            set => _lockerID = value;
        }
    } 
}

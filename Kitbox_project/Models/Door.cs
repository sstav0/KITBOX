using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kitbox_project.Models
{
    /// <summary>
    /// Creates a door with specified dimensions, color, and material.
    /// <list type="bullet">
    /// <item><description>To get the color of the door use Door.Color </description>
    /// </item>
    ///<item><description>To get the material of the door use Door.Material </description>
    ///</item>
    ///<item><description>To get the height of the door use Door.Height </description>
    ///</item>
    ///<item><description>To get the width of the door use Door.Width </description>
    ///</item>
    /// <item><description>To set the color of the door, use the method <c>SetColor(string color)</c>.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class Door
    {
        private string _color;
        private string _material;
        private int _width;
        private int _height;

        public Door(string color, string material, int width, int heigth) 
        {
            this._color = color;
            this._material = material;
            this._width = width; 
            this._height = heigth;   
        }

        public string Color
        {
            get => _color;
            set => _color = value;
        }

        public string Material
        {
            get => _material;
            set => _material = value;
        }

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", _color, _material, _width.ToString(), _height.ToString());
        }
    }
}

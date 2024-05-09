using Kitbox_project.Models;
using Kitbox_project.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


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
        private DatabaseCatalog databaseCatalog = new DatabaseCatalog("storekeeper", "storekeeper");
        public readonly Dictionary<string, int> partsToBuildALockerDict = new Dictionary<string, int>
        {
            {"PAG", 2 }, //Side Panel
            {"TAS", 4 }, //Vertical Batten
            {"PAR", 1 }, //Back Panel
            {"PAH", 2 }, //Horizontal Panel
            {"POR", 2 }, //Door
            {"TRG", 4 }, //Side Crossbar
            {"TRF", 2 }, //Front Crossbar
            {"TRR", 2 }  //Back Crossbar
        };
        private Dictionary<string, int> partsRegisteredForCabinet = new Dictionary<string, int>();

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

        private Dictionary<string,object> SelectedValues()
        {
            bool isDoor = false;
            if (this.Door != null) { isDoor = true; }
            Dictionary<string, object> selectedValues = new Dictionary<string, object>
            {
                { "Height"              ,this.Height },
                { "Depth"               ,this.Depth },
                { "Width"               ,this.Width },
                { "Door Color"          ,this.Door.Color },
                { "Door Material"       ,this.Door.Material },
                { "Door"                ,isDoor },
                //{ "Angle Color"         ,this.Depth },
                { "Panel Color"         ,this.Color }
            };
            return selectedValues;
        } 

        /// <summary>
        /// Retrieves the catalog reference based on the specified two-letter reference code.
        /// </summary>
        /// <param name="twoletterRef">A two-letter reference code used to retrieve the catalog reference.</param>
        /// <returns>
        /// A string representing the catalog reference.
        /// If a matching reference is found, it returns the reference code.
        /// If the two-letter reference code is "DOORBOOL" and a door exists, it returns "1" indicating the presence of a door.
        /// If no matching reference is found or if the two-letter reference code is "DOORBOOL" and no door exists, it returns an empty string.
        /// </returns>
        /// <remarks>
        /// This method asynchronously queries the catalog for values based on selected parameters such as height, depth, width, color, and material.
        /// If a door is present, additional door-related parameters are considered.
        /// The method searches for a matching catalog reference using the provided two-letter reference code.
        /// If the two-letter reference code is "DOORBOOL" and a door exists, it returns "1".
        /// <para>
        /// Examples of possible values for the two-letter reference code include:
        /// <list type="bullet">
        ///     <item>"PAG" for the side panel reference,</item>
        ///     <item>"TAS" for the vertical batten reference,</item>
        ///     <item>"PAR" for the back panel reference,</item>
        ///     <item>"PAH" for the horizontal panel reference,</item>
        ///     <item>"POR" for the door reference,</item>
        ///     <item>"TRG" for the side crossbar reference,</item>
        ///     <item>"TRF" for the front crossbar reference, and</item>
        ///     <item>"TRR" for the back crossbar reference.</item>
        /// </list>
        /// Additionally, it explains the special case when the two-letter reference code is "DOORBOOL" which checks for the presence of a door.
        /// </para>
        /// </remarks>
        public async Task<string> GetCatalogRef(string threeLetterRef)
        {
            Dictionary<string,object> selectedValues = SelectedValues();

            Catalog catalog = new Catalog(databaseCatalog, selectedValues);
            string returnString = "";

            var data = await catalog.GetValues();
            Dictionary<string, int> references = data.Item1;

            foreach (string item in references.Keys)
            {
                if (item.Contains(threeLetterRef, StringComparison.OrdinalIgnoreCase)) 
                {
                    returnString = item;
                }  
                else if (threeLetterRef == "DOORBOOL" && this.Door != null)
                {
                    returnString = "1";
                }
            }
            return returnString;
        }

        public async Task<int> GetNumberOfPartsAvailable(string threeLetterRef)
        {
            Catalog catalog = new Catalog(databaseCatalog, SelectedValues());
            int returnValue = 0;

            var data = await catalog.GetValues();
            Dictionary<string, int> references = data.Item1;
            foreach (string item in references.Keys)
            {
                if (item.Contains(threeLetterRef, StringComparison.OrdinalIgnoreCase))
                {
                    returnValue = references[item];
                    break;
                }
            }
            return returnValue;
        }

        private async Task<bool> IsPartAvailable(string threeLetterRef, int quantityRegistered = 0)
        {
            Dictionary<string, object> selectedValues = SelectedValues();
            Catalog catalog = new Catalog(databaseCatalog, selectedValues);

            bool returnValue = false;

            var data = await catalog.GetValues();
            Dictionary<string, int> partsAvailabilityDict = data.Item1;

            string catalogRef = await this.GetCatalogRef(threeLetterRef);

            if (partsAvailabilityDict.ContainsKey(catalogRef) && partsToBuildALockerDict.ContainsKey(threeLetterRef)) 
            { 
                if (quantityRegistered <= 0 && partsAvailabilityDict[catalogRef] >= partsToBuildALockerDict[threeLetterRef]) 
                {
                    returnValue = true;
                }
                else if (quantityRegistered > 0 && partsAvailabilityDict[catalogRef] >= quantityRegistered)
                {
                    returnValue = true;
                }
            }
            //In case we ask for the Door Reference and the locker doesn't contain any door
            else if (!partsAvailabilityDict.ContainsKey(catalogRef) && partsToBuildALockerDict.ContainsKey(threeLetterRef))
            {
                returnValue = true;
            }

            return returnValue;
        }

        public async Task<(bool, Dictionary<string, int>)> ArePartsAvailable(Dictionary<string, int> registeredCatalogRef = null)
        {
            bool returnBool = true;
            //setup registererRef Dictionary
            Dictionary<string, int> registeredRef = new Dictionary<string, int>();
            if (registeredCatalogRef != null)
            {
                registeredRef = registeredCatalogRef;
            }
            else
            {
                registeredRef = partsToBuildALockerDict;
            }

            //for every parts needed in a locker
            foreach (string threeLetterRef in partsToBuildALockerDict.Keys)
            {
                //Get the catalog reference for this part
                string catalogRef = await GetCatalogRef(threeLetterRef);
                //if the reference isn't already used for other lockers of this same cabinet
                if (!registeredRef.ContainsKey(catalogRef)) 
                {
                    registeredRef.Add(catalogRef, 0);
                }
                registeredRef[catalogRef] += partsToBuildALockerDict[threeLetterRef];

                if (!await IsPartAvailable(threeLetterRef, registeredRef[catalogRef]))
                {
                    returnBool = false;
                }
            }
            return (returnBool, registeredRef);
        }
    }
}

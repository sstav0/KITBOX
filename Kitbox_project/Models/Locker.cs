using Kitbox_project.Models;
using Kitbox_project.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;


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
        public Dictionary<string, int> partsRegisteredForLocker = new Dictionary<string, int>();
        public bool partsAvailabilityBool = false;
        public readonly Dictionary<string, int> partsToBuildALockerDict = new Dictionary<string, int>
        {
            {"PAG", 2 }, //Side Panel
            {"TAS", 4 }, //Vertical Batten
            {"PAR", 1 }, //Back Panel
            {"PAH", 2 }, //Horizontal Panel
            {"POR", 2 }, //Door
            {"TRG", 4 }, //Side Crossbar
            {"TRF", 2 }, //Front Crossbar
            {"TRR", 2 }, //Back Crossbar
            {"COU", 2 }, //Coupelle
        };

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
            return String.Format("{0} {1} {2} {3} {4} {5}", _height.ToString(), _width.ToString(), _depth.ToString(), _color, _door != null ? _door.ToString() : null, this._price.ToString());
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

        private Dictionary<string, object> SelectedValues()
        {
            bool isDoor = false;
            string doorColor = null;
            string doorMaterial = null;
            if (this.Door != null) { isDoor = true; doorColor = this.Door.Color; doorMaterial = this.Door.Material; }

            Dictionary<string, object> selectedValues = new Dictionary<string, object>
            {
                { "Height"              ,this.Height },
                { "Depth"               ,this.Depth },
                { "Width"               ,this.Width },
                { "Door Color"          ,doorColor },
                { "Door Material"       ,doorMaterial },
                { "Door"                ,isDoor },
                { "Panel Color"         ,this.Color }
                //{ "Angle Color"         ,this.Depth }
            };
            return selectedValues;
        }

        public async Task<double> GetPrice()
        {
            
            if (partsRegisteredForLocker == null) { Price = 0; return 0; }
            else
            {
                Price = 0;
                Dictionary<string, string> condition = new Dictionary<string, string>();    
                List<string> column = new List<string>() { "Price" };

                List<Dictionary<string,string>> data = new List<Dictionary<string,string>>();

                foreach(string part in partsRegisteredForLocker.Keys)
                {
                    if (part != null)
                    {
                        Debug.WriteLine(part);
                    }
                }

                foreach(string part in partsRegisteredForLocker.Keys)
                {
                    if (part != null && !string.IsNullOrWhiteSpace(part))
                    {
                        Debug.WriteLine(part);
                        condition.Clear();
                        condition.Add("Code", part);
                        data = await databaseCatalog.GetCatalogData(condition, column);
                        string price = data[0]["Price"];

                        foreach (string threeLetterRef in partsToBuildALockerDict.Keys)
                        {
                            if (part.Contains(threeLetterRef, StringComparison.OrdinalIgnoreCase))
                            {
                                Price += Math.Round(partsToBuildALockerDict[threeLetterRef] * float.Parse(price),2);
                            }
                        }
                    }
                }
                return Price;
            }
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
            Dictionary<string, object> selectedValues = SelectedValues();

            Catalog catalog = new Catalog(databaseCatalog, selectedValues);
            string returnString = "";

            var data = await catalog.GetValues();
            Dictionary<string, int> references = data.Item1;

            foreach (string item in references.Keys)
            {
                if (item.Contains(threeLetterRef, StringComparison.OrdinalIgnoreCase))
                {
                    if (threeLetterRef == "PAG" || threeLetterRef == "PAR" || threeLetterRef == "PAH") 
                    {
                        if (item.Substring(item.Length - 2).Contains("Bl", StringComparison.OrdinalIgnoreCase) && this.Color.Contains("White", StringComparison.OrdinalIgnoreCase)) 
                        {
                            returnString = item;
                        }
                        if (item.Substring(item.Length - 2).Contains("Br", StringComparison.OrdinalIgnoreCase) && this.Color.Contains("Brown", StringComparison.OrdinalIgnoreCase))
                        {
                            returnString = item;
                        }
                    }
                    else if (threeLetterRef == "POR")
                    {
                        if (item.Substring(item.Length - 2).Contains("Ve", StringComparison.OrdinalIgnoreCase) && this.Door.Color.Contains("Transparent", StringComparison.OrdinalIgnoreCase))
                        {
                            returnString = item;
                        }
                        else if (item.Substring(item.Length - 2).Contains("Bl", StringComparison.OrdinalIgnoreCase) && this.Door.Color.Contains("White", StringComparison.OrdinalIgnoreCase))
                        {
                            returnString = item;
                        }
                        else if (item.Substring(item.Length - 2).Contains("Br", StringComparison.OrdinalIgnoreCase) && this.Door.Color.Contains("Brown", StringComparison.OrdinalIgnoreCase))
                        {
                            returnString = item;
                        }
                    }
                    else
                    {
                        returnString = item;
                    }
                }
                else if (threeLetterRef == "DOORBOOL" && this.Door != null)
                {
                    returnString = "1";
                }
            }
            return returnString;
        }

        /// <summary>
        /// Retrieves the number of available parts in the catalog based on the provided three-letter reference code.
        /// </summary>
        /// <param name="threeLetterRef">The reference code of the part, typically a three-letter code.</param>
        /// <returns>The number of available parts corresponding to the provided reference code in the catalog.</returns>
        /// <remarks>
        /// This method asynchronously retrieves data from the catalog and searches for the provided three-letter 
        /// reference code within the catalog references. It returns the number of available parts associated with 
        /// the matching reference code. If no matching reference is found, it returns zero.
        /// </remarks>
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

        /// <summary>
        /// Checks the availability of a part in the catalog based on its reference and optional registered quantity.
        /// </summary>
        /// <param name="threeLetterRef">The reference code of the part, typically a three-letter code.</param>
        /// <param name="quantityRegistered">The quantity of the part registered. Default is 0.</param>
        /// <returns>True if the part is available according to the catalog and optional registered quantity; otherwise, false.</returns>
        /// <remarks>
        /// This method asynchronously retrieves data from the catalog and compares it with the provided reference code 
        /// and registered quantity to determine if the part is available. If the part is found in the catalog and either no 
        /// quantity is registered or the registered quantity is less than or equal to the available quantity, the method 
        /// returns true. If the provided reference code corresponds to a part not found in the catalog and the part is 
        /// expected in the locker, the method also returns true to handle cases where the catalog reference might not exist 
        /// but the part is still required for the locker assembly.
        /// </remarks>
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


        /// <summary>
        /// Checks the availability of multiple parts required for building a locker assembly.
        /// </summary>
        /// <param name="registeredCatalogRef">Optional. A dictionary containing the registered catalog references 
        /// along with their quantities. If null, the default dictionary containing parts required to build the locker 
        /// will be used.</param>
        /// <returns>A tuple containing a boolean indicating whether all parts are available, and a dictionary 
        /// containing the registered catalog references along with their updated quantities.</returns>
        /// <remarks>
        /// This method iterates over each part required to build a locker and checks its availability using the 
        /// <see cref="IsPartAvailable"/> method. It updates the quantity of registered catalog references accordingly 
        /// based on the parts required for the locker assembly. If any part is not available, the method returns false; 
        /// otherwise, it returns true along with the updated dictionary of registered catalog references.
        /// </remarks>
        public async Task<(bool, Dictionary<string, int>)> ArePartsAvailable(List<Dictionary<string, int>> registeredCatalogRef = null)
        {
            bool returnBool = true;
            //setup registererRef Dictionary
            Dictionary<string, int> registeredRef = new Dictionary<string, int>();
            Dictionary<string, int> returnRegisteredRefDict = new Dictionary<string, int>();

            if (registeredCatalogRef != null)
            {
                registeredRef = registeredCatalogRef
                    .SelectMany(dict => dict)
                    .GroupBy(kv => kv.Key, kv => kv.Value)
                    .ToDictionary(g => g.Key, g => g.Sum());
            }
            else
            {
                registeredRef = partsToBuildALockerDict;
            }

            //for every parts needed in a locker
            foreach (string threeLetterRef in partsToBuildALockerDict.Keys.ToList())
            {
                //Get the catalog reference for this part
                string catalogRef = await GetCatalogRef(threeLetterRef);
                if (catalogRef != null && !returnRegisteredRefDict.ContainsKey(catalogRef)) { returnRegisteredRefDict.Add(catalogRef, partsToBuildALockerDict[threeLetterRef]); }
               

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
            partsRegisteredForLocker = returnRegisteredRefDict;
            partsAvailabilityBool = returnBool;
            return (returnBool, returnRegisteredRefDict);
        }
    }
}

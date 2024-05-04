using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
using System.Linq;
using Kitbox_project.DataBase;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TEST_ORM
{
    /// <summary> THE ONLY METHOD YOU NEED IN THIS CLASS IS <c>GetValues</c>. OTHER METHODS are used INTERNALLY to format the data.
    /// TO USE THIS CLASS, YOU NEED TO INSTANTIATE IT WITH A <c>DatabaseCatalog</c> OBJECT AND A DICTIONARY OF PARAMETERS. THE PARAMETERS ARE THE FILTERS THAT THE CLIENT HAS SELECTED.
    /// THEN YOU CALL THE <c>GetValues</c> METHOD. THIS METHOD RETURNS A TUPLE WHERE THE FIRST ELEMENT IS A DICTIONARY WHERE EACH KEY IS THE REFERENCE OF A PRODUCT AND THE VALUE IS THE QUANTITY OF THIS PRODUCT IN THE CATALOG TABLE. THE SECOND ELEMENT IS A DICTIONARY WHERE EACH KEY
    /// </summary>
    internal class Catalog_v3
    {
        private Dictionary<string, string> _regexes = new Dictionary<string, string> { { "Color", @"^(.*?)_color$" }, { "Material", @"^(.*?)_material" } };
        private bool _requiresDoor;
        private int? _totalHeight;
        private int _maxHeight;
        private Dictionary<string, string> _colorDetails = new Dictionary<string, string>();
        private Dictionary<string, string> _materialDetails = new Dictionary<string, string>();
        private Dictionary<string, object> param;
        //private readonly DatabaseCatalog _databaseCatalog;
        private readonly DatabaseCatalog _databaseCatalog;


        //public Catalog_v3(DatabaseCatalog databaseCatalog, Dictionary<string, object> param)
        //{
        //    _databaseCatalog = databaseCatalog;
        //    this.param = param;
        //}

        public Catalog_v3(DatabaseCatalog databaseCatalog, Dictionary<string, object> param)
        {
            _databaseCatalog = databaseCatalog;
            this.param = param;
        }
        public int MaxHeight { get => _maxHeight; set => _maxHeight = value; }
        public Dictionary<string, string> Regexes { get => _regexes; set => _regexes = value; }
        public bool RequiresDoor { get => _requiresDoor; set => _requiresDoor = value; }
        public int? TotalHeight { get => _totalHeight; set => _totalHeight = value; }
        public Dictionary<string, string> ColorDetails { get => _colorDetails; set => _colorDetails = value; }
        public Dictionary<string, string> MaterialDetails { get => _materialDetails; set => _materialDetails = value; }

        /// <summary>
        /// This method checks if the query contains a door. If it does, it sets the <c>RequiresDoor</c> property to <c>true</c>. Otherwise, it sets it to <c>false</c>.
        /// </summary>
        private void CheckDoor()
        {
            if (this.param.ContainsKey("Door"))
            {
                if (this.param["Door"] is string) //check if the door is selected
                {
                    RequiresDoor = this.param["Door"] == "True" ? true : false;
                }
                else
                {
                    RequiresDoor = this.param["Door"] != null ? (bool)this.param["Door"] : false;
                }
            }
        }

        /// <summary>
        /// This method calculates the maximum cabinet height among the items in the catalog table.
        /// </summary>
        private void GetMaxHeight(List<Dictionary<string, string>> res)
        {
            MaxHeight = 0;
            foreach (var item in res)
            {
                if (item.TryGetValue("Cabinet_Height", out var cabinetHeightStr) && int.TryParse(cabinetHeightStr, out var cabinetHeight))
                {
                    if (cabinetHeight > MaxHeight)
                    {
                        MaxHeight = cabinetHeight;
                    }
                }
            }
        }

        /// <summary>
        /// This method checks if the query contains a total height. If it does, it sets the <c>TotalHeight</c> property to the value of the total height. Otherwise, it sets it to <c>0</c>.
        /// </summary>
        private void CheckTotalHeight()
        {
            if (this.param.ContainsKey("TotalHeight"))
            {
                if (this.param["TotalHeight"] is string)
                {
                    int.TryParse(this.param["TotalHeight"]?.ToString(), out int intValue);
                    TotalHeight = intValue;
                }
                else
                {
                    TotalHeight = (int)this.param["TotalHeight"];
                }
            }
            else
            {
                TotalHeight = 0;
            }
        }

        /// <summary>
        /// This method checks if the query contains a color or material filter. If it does, it assigns the color or material to the corresponding element.
        /// </summary>
        private void CheckColorMaterialFilter()
        {
            foreach (var item in this.param)
            {
                if (item.Value != null) {
                    var key = item.Key;
                    var value = item.Value.ToString();

                    if (key.EndsWith("_color"))
                    {
                        // Extract element name and assign the color to it
                        var element = key.Substring(0, key.IndexOf("_color"));
                        ColorDetails[element] = value;
                    }
                    else if (key.EndsWith("_material"))
                    {
                        // Extract element name and assign the material to it
                        var element = key.Substring(0, key.IndexOf("_material"));
                        MaterialDetails[element] = value;
                    }
                }

            }
        }

        /// <summary>
        /// This method returns a tuple where the first element is a dictionary where each key is the reference of a product and the value is the quantity of this product in the catalog table. The second element is a dictionary where each key 
        /// is the name of a column in the catalog table and the value is a list of all the possible values for this column. The list is then used to display the values of each key of the returned dictionary in the client application.
        /// The list is then used to display the values of each key of the returned dictionary in the client application.
        /// Examples of parameters <paramref name="param"/> are :
        /// <list type="bullet">
        /// <item>
        /// <description> Here every key has a value, <c>null</c> means that it hasn't been yet selected by the client.
        /// <code>{ { "Width", 52 }, { "Depth", null }, { "Panel_color", "Brown" }, { "Height", 52 }, { "Door", true }, {"Door_color", "Glass"} }</code>
        /// </description>
        /// </item>
        /// <item>
        /// <description> Same as the previous example, but with the key <c>"Depth"</c> has been deleted. The result will be the same.
        /// <code>{ { "Width", 52 }, { "Panel_color", "Brown" }, { "Height", 52 }, { "Door", true }, {"Door_color", "Glass"} }</code>
        /// </description>
        /// </item>
        /// <item>
        /// <description> Here it will return every possible value in the database.
        /// <code>{ { "Width", null }, { "Depth", null }, { "Panel_color", null }, { "Height", null }, { "Door", true }, { "Door_color", null } }</code>
        /// </description>
        /// </item>
        /// <item>
        /// <description> The result will be the same as the previous example.
        /// <code>{}</code>
        /// </description>
        /// </item>
        /// <item>
        /// <description> This is the most complete example. You typically use this when the client has selected every possible value and you want to send the reference of the product to the order page.
        /// <code>{ { "Width", 52 }, { "Depth", 32 }, { "Panel side_color", "Brown" }, { "Panel horizontal_color", "Brown" }, { "Panel back_color", "Brown" }, { "Door_color", "Transparent" }, { "Angle_color", "Black" }, { "TotalHeight", 209 }, { "Height", 32 }, { "Door", true } }</code>
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="dims">The columns that set the dimensions of the cabinet (you don't need to touch it). By default : <code>{ "Width", "Height", "Depth" }</code> </param>
        /// <param name="columns">The columns that the query to the database will use. Example : <code>{ "Width", "Height"}</code>By default, these columns will be selected : <code>{ "Reference", "Code", "Width", "Height", "Depth", "Color", "Material", "Cabinet_Height", "Price", "Quantity" }</code></param>
        /// <returns>tuple[0] informations for the order and to check part number; tuple[1] informations for the pickers</returns>
        public async Task<(Dictionary<string, int>, Dictionary<string, List<string>>)> GetValues(List<string> columns = null, List<string> dims = null)
        {
            var request = new Dictionary<string, string>();

            if (dims == null)
            {
                dims = new List<string> { "Width", "Height", "Depth" };
            }

            if (columns == null)
            {
                columns = new List<string> { "Reference", "Code", "Width", "Height", "Depth", "Color", "Material", "Cabinet_Height", "Price", "Quantity" };
            }

            foreach (var item in dims)
            {
                if (param.ContainsKey(item) && param[item] != null)
                {
                    request.Add(item, param[item].ToString());
                }
            }

            CheckDoor();
            CheckTotalHeight();
            CheckColorMaterialFilter();

            //var res = _databaseCatalog.GetCatalogData(request, columnsParameter: columns);
            var taskRequestDB = _databaseCatalog.GetCatalogData(request, columnsParameter: columns);
            var result = await taskRequestDB;
            var filtered = FilterItems(result);
            var orderAns = FormatValues(filtered);
            var pickerAns = PickerValues(filtered, dims);

            return (orderAns, pickerAns);
        }


        private List<Dictionary<string, string>> FilterItems(List<Dictionary<string, string>> items)
        {
            GetMaxHeight(items);

            var filteredItems = new List<Dictionary<string, string>>();
            var eligibleItems = new List<Dictionary<string, string>>();

            // Calculate the maximum height among items
            int maxHeightAmongItems = items
                .Where(item => item.TryGetValue("Height", out var heightStr) && int.TryParse(heightStr, out int height))
                .Select(item => int.Parse(item["Height"]))
                .DefaultIfEmpty(0) // Use 0 if there are no items with "Height"
                .Max();

            foreach (var item in items)
            {
                // Color filtering
                if (ColorDetails.ContainsKey(item["Reference"].ToString()))
                {
                    if (item.ContainsKey("Color") && !item["Color"].Equals(ColorDetails[item["Reference"].ToString()], StringComparison.OrdinalIgnoreCase))
                    {
                        continue; // Skip this item if color doesn't match
                    }
                }

                // Material filtering
                if (MaterialDetails.ContainsKey(item["Reference"].ToString()))
                {
                    if (item.ContainsKey("Material") && !item["Material"].Equals(MaterialDetails[item["Reference"].ToString()], StringComparison.OrdinalIgnoreCase))
                    {
                        continue; // Skip this item if material doesn't match
                    }
                }

                // Door requirement filtering
                if (!RequiresDoor && item.ContainsKey("Reference") && item["Reference"].Contains("Door", StringComparison.OrdinalIgnoreCase))
                {
                    //var hasDoor = item["Reference"].Contains("Door", StringComparison.OrdinalIgnoreCase);
                    if (!RequiresDoor)
                    {
                        continue; // Skip this item if door requirement doesn't match
                    }
                }

                // Height filtering
                if (item.TryGetValue("Height", out var heightStr) && int.TryParse(heightStr, out var height))
                {
                    if (height + TotalHeight > MaxHeight)
                    {
                        continue; // Skip this item if it's too tall
                    }
                }

                // Cabinet height filtering
                if (item.TryGetValue("Cabinet_Height", out var cabinetHeightStr) && int.TryParse(cabinetHeightStr, out var cabinetHeight))
                {
                    if (cabinetHeight >= TotalHeight + maxHeightAmongItems)
                    {
                        eligibleItems.Add(item); // Add this item to the list of eligible items
                        continue;

                    }
                    continue;
                }
                // This item passes all criteria, so add it to the list
                filteredItems.Add(item);
            }

            // Now find the item with the smallest "Cabinet_Height" among eligible items
            var itemWithSmallestCabinetHeight = eligibleItems
                .Where(item => item.ContainsKey("Cabinet_Height"))
                .OrderBy(item => int.Parse(item["Cabinet_Height"]))
                .FirstOrDefault();

            filteredItems.Add(itemWithSmallestCabinetHeight); //Add the angle with the smallest height among the eligible angles

            return filteredItems;
        }

        /// <summary>
        /// This method sorts the values of the dictionary <paramref name="param"/>. It sorts the values of the keys that are not in the <paramref name="ignoredColumns"/> list. If the first element of the list is numeric, it sorts the list as integers and removes duplicates. Otherwise, it leaves the list as is.
        /// </summary>
        /// <param name="param">The dictionary to sort (it typically is the result of the database query).</param>
        /// <param name="ignoredColumns">The columns (=keys) to don't sort.</param>
        /// <returns>The sorted dictionary.</returns>
        private Dictionary<string, List<string>> SortList(Dictionary<string, List<string>> param, List<string> ignoredColumns = null)
        {
            ignoredColumns ??= new List<string> { "Quantity", "Price", "Color", "Material" };

            var sortedResult = new Dictionary<string, List<string>>();

            foreach (var item in param)
            {
                if (ignoredColumns.Contains(item.Key))
                {
                    sortedResult.Add(item.Key, item.Value);
                }
                else
                {
                    // Check if the first element is numeric to determine sort behavior
                    if (item.Value.Count > 0 && int.TryParse(item.Value.First(), out _))
                    {
                        // If numeric, sort as integers and remove duplicates
                        var numericSortedSet = new SortedSet<int>(item.Value.Select(int.Parse));
                        sortedResult.Add(item.Key, numericSortedSet.Select(n => n.ToString()).ToList());
                    }
                    else
                    {
                        // Leave as is for non-numeric lists
                        // If uniqueness is also required for non-numeric, consider applying a similar pattern with SortedSet or a distinct operation.
                        sortedResult.Add(item.Key, item.Value.Distinct().ToList()); // Distinct() is used here to remove duplicates for consistency
                    }
                }
            }

            return sortedResult;
        }

        /// <summary>
        /// This method formats the values of the dictionary <paramref name="res"/>. It returns a dictionary where the key is the code of the product and the value is the quantity of this product.
        /// </summary>
        /// <param name="res"> The dictionary to format. It typically is the result of the database query.
        /// </param>
        /// <returns> The formatted dictionary.
        /// </returns>
        private Dictionary<string, int> FormatValues(List<Dictionary<string, string>> res)
        {
            string columnKey = "Code";
            string columnValue = "Quantity";

            var retVal = new Dictionary<string, int>();
            string code = null;

            foreach (var item in res)
            {
                foreach (var key in item.Keys)
                {
                    if (key == columnKey)
                    {
                        code = item[key];
                    }

                    if (key == columnValue)
                    {
                        if (int.TryParse(item[key], out int intValue))
                        {
                            retVal[code] = intValue;
                        }
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// This method returns a dictionary where the key is the name of a column in the catalog table and the value is a list of all the possible values for this column. The list is then used to display the values of each key of the returned dictionary in the client application.
        /// </summary>
        /// <param name="res"> The dictionary to format. It typically is the result of the database query.
        /// </param>
        /// <param name="dims"> The columns that set the dimensions of the cabinet (you don't need to touch it). By default : <code>{ "Width", "Height", "Depth" }</code>
        /// </param>
        /// <returns> The formatted dictionary.
        /// </returns>
        private Dictionary<string, List<string>> PickerValues(List<Dictionary<string, string>> res, List<string> dims)
        {
            var retVal = new Dictionary<string, List<string>>();

            string ref_ = null;

            foreach (var item in res)
            {
                foreach (var key in item.Keys)
                {
                    if (key == "Reference")
                    {
                        ref_ = item[key];
                    }

                    if (key == "Color" || key == "Material")
                    {
                        if (retVal.ContainsKey(ref_ + " " + key))
                        {
                            retVal[ref_ + " " + key].Add(item[key]);
                        }
                        else
                        {
                            retVal[ref_ + " " + key] = new List<string> { item[key] };
                        }
                    }

                    if (dims.Contains(key))
                    {
                        if (retVal.ContainsKey(key))
                        {
                            retVal[key].Add(item[key]);
                        }
                        else
                        {
                            retVal[key] = new List<string> { item[key] };
                        }
                    }
                }
            }
            return SortList(retVal, ignoredColumns: new List<string> { });
        }

        public async Task<Dictionary<string, List<string>>> GetPickerValues()
        {
            List<string> availableWidth = new List<string>();
            List<string> availableHeight = new List<string>();  
            List<string> availableDepth = new List<string>();
            List<string> availablePanelColor = new List<string>();
            List<string> availableAngleColor = new List<string>();  
            List<string> availableDoorColor = new List<string>();
            List<string> availableDoorMaterial = new List<string>();

            var newData = await GetValues();
            Dictionary<string, List<string>> partDict = newData.Item2;

            bool panelColorflag = false;

            foreach (var part in partDict)
            {
                if (part.Value != null)
                {
                    if (part.Key.Contains("Panel", StringComparison.OrdinalIgnoreCase) && part.Key.Contains("color", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!panelColorflag)
                        {
                            availablePanelColor = part.Value;
                            panelColorflag = true;
                        }
                        else
                        {
                            availablePanelColor = availablePanelColor.Intersect(part.Value).ToList();
                        }
                    }
                }
            }

            if (partDict.ContainsKey("Height") && partDict["Height"] != null)         { availableHeight = partDict["Height"]; }
            if (partDict.ContainsKey("Depth") && partDict["Depth"] != null)          { availableDepth = partDict["Depth"]; }
            if (partDict.ContainsKey("Width") && partDict["Width"] != null)          { availableWidth = partDict["Width"]; }
            if (partDict.ContainsKey("Door Color") && partDict["Door Color"] != null)     { availableDoorColor = partDict["Door Color"]; }
            if (partDict.ContainsKey("Door Material") && partDict["Door Material"] != null)  { availableDoorMaterial = partDict["Door Material"]; }
            if (partDict.ContainsKey("Angle Color") && partDict["Angle Color"]!= null)     {availableAngleColor = partDict["Angle Color"]; }

            Dictionary<string,List<string>> returnValues = new Dictionary<string, List<string>> {
                                { "Width", availableWidth }, { "Depth", availableDepth },
                                { "Panel_color", availablePanelColor }, { "Height", availableHeight },
                                { "Door_color", availableDoorColor }, {"Angle_color", availableAngleColor }, 
                                {"Door_material", availableDoorMaterial}};

            return returnValues;
        }     
    }
}

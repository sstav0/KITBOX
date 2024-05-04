using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kitbox_project.DataBase;
using Kitbox_project.Utilities;

namespace Kitbox_project.Models
{
    /// <summary> The ONLY method you need to use in this class is <c>GetValues</c>. OTHER METHODS are used INTERNALLY to format the data.
    /// </summary>
    public class Catalog
    {
        private readonly DatabaseCatalog _databaseCatalog;
        public Catalog(DatabaseCatalog databaseCatalog)
        {
            _databaseCatalog = databaseCatalog;
        }

        /// <summary>
        /// This method returns a dictionary of lists of objects, where each key represents a column name and each value is a list of unique values for that column.
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
        /// </list>
        /// </summary>
        /// <param name="param">The dictionary containing the values selected by the client. Example : <code>{ { "Width", 52 }, { "Depth", null }, { "Panel_color", "Brown" }, { "Height", 52 }, { "Door", true }, {"Door_color", "Glass"} }</code> </param>
        /// <param name="columns">The columns that will be selected. Example : <code>{ "Width", "Height"}</code>By default, these columns will be selected : <code>{ "Width", "Height", "Depth", "Panel_color", "Door_color", "Angle_color" }</code></param>
        /// <returns>A dictionary that can directly be used to display the options in the client interface</returns>
        public async Task<Dictionary<string, List<object>>> GetValues(Dictionary<string, object> param, List<string> columns = null)
        {
            if (columns == null)
            {
                columns = new List<string> { "Width", "Height", "Depth", "Panel_color", "Door_color", "Angle_color", "Door_material" }; //columns that will be selected by default in the database
            }

            var ans = new List<Dictionary<string, string>>();

            var selectedValues = CorrectValues(param);

            (bool door, List<int> maxs) = await CheckDoor(param);

            if (selectedValues.Count == 0)
            {
                ans = await _databaseCatalog.LoadAll(columns);
                return FormatValues(ans, door, maxs[0], maxs[1], columns);
            }
                        
            ans = await _databaseCatalog.GetCatalogData(selectedValues, columns);

            return FormatValues(ans, door, maxs[0], maxs[1], columns);
        }

        /// <summary>
        /// This method removes the null and bool values from the dictionary <paramref name="selectedValues"/> and returns a new dictionary with the correct values.
        /// </summary>
        /// <param name="selectedValues"></param>
        /// <returns></returns>
        public Dictionary<string, string> CorrectValues(Dictionary<string, object> selectedValues) //
        {
            return selectedValues // Remove null and bool values from the dictionary
                .Where(item => !(item.Value is bool) && item.Value != null)
                .ToDictionary(item => item.Key, item => Utils.ValueToString(item.Value));
        }

        /// <summary>
        /// This method sorts the values of the dictionary <paramref name="param"/> and returns a new dictionary with the sorted values.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Dictionary<string, List<object>> SortList(Dictionary<string, List<object>> param)
        {
            var res = new Dictionary<string, List<object>>();

            foreach (var item in param)
            {
                if (item.Value.Count > 0 && item.Value[0] is int) //check if the list contains integers
                {
                    var list = new List<int>(); //create a new list to store the unique values

                    list = Utils.ConvertToInt(item.Value); //get the list of values
                    HashSet<int> set = new HashSet<int>(list); //create a hashset to store the unique values
                    list = set.ToList(); //convert the hashset back to a list
                    list.Sort(); //sort the list

                    res.Add(item.Key, Utils.ConvertToObject(list)); //add the unique values to the dictionary
                }
                else if (item.Value.Count > 0 && item.Value[0] is string)
                {

                    var list = new List<string>(); //create a new list to store the unique values

                    list = item.Value.Cast<string>().ToList(); //get the list of values
                    HashSet<string> set = new HashSet<string>(list); //create a hashset to store the unique values
                    list = set.ToList(); //convert the hashset back to a list
                    list.Sort(); //sort the list

                    res.Add(item.Key, list.Cast<object>().ToList()); //add the unique values to the dictionary

                }
                else
                {
                    res.Add(item.Key, item.Value); //add the non-integer values to the dictionary
                }
            }

            return res;
        }

        /// <summary>
        /// This method checks if the door is in <paramref name="param"/> and returns a tuple with a boolean (<c>true</c> if there is a door and <c>false</c> if there isn't)  and a list of two integers.
        /// The FIRST INTEGER is the maximum height of the doors and the SECOND INTEGER is the maximum width of the doors.
        /// </summary>
        /// <param name="param">The same dictionary as the <c>GetValues</c> method.</param>
        /// <param name="doorRealWidthFactor">The max width possible with two doors</param>
        /// <returns><c>(bool, List(height, width)</c> </returns>
        public async Task<(bool, List<int>)> CheckDoor(Dictionary<string, object> param, int doorRealWidthFactor = 2)
        {
            bool door = false;

            if (param.ContainsKey("Door"))
            {
                if (param["Door"] is string) //check if the door is selected
                {
                    door = param["Door"] == "True" ? true : false;
                }
                else
                {
                    door = param["Door"] != null ? (bool)param["Door"] : false;
                }
            }

            var doors = await _databaseCatalog.GetData(new Dictionary<string, string> { { "Reference", "Door" } });

            var heights = new List<int>();
            var widths = new List<int>();

            foreach (var item in doors)
            {
                foreach (var entry in item)
                {
                    bool isIntValue = int.TryParse(entry.Value?.ToString(), out int intValue); //check if the value is an integer
                    if (entry.Key == "Height" && isIntValue)
                    {
                        heights.Add(intValue);
                    }
                    else if (entry.Key == "Width" && isIntValue)
                    {
                        widths.Add(intValue * doorRealWidthFactor);
                    }
                }
            }

            return (door, new List<int> { heights.Max(), widths.Max() });
        }

        public Dictionary<string, List<object>> FormatValues(List<Dictionary<string, string>> values, bool door, int widthMax, int heightMax, List<string> columns)
        {
            // Initialize the result dictionary with empty lists for each column
            var res = columns.ToDictionary(column => column, column => new List<object>());

            foreach (var elem in values)
            {
                foreach (var column
                    in columns)
                {
                    // Check if the current element has the column
                    if (elem.TryGetValue(column, out var value) && !string.IsNullOrEmpty(value))
                    {
                        // Handle numeric values specifically for "Width" and "Height" due to additional checks
                        if (int.TryParse(value, out int intValue))
                        {
                            if (!door || (column == "Width" && intValue <= widthMax) || (column == "Height" && intValue <= heightMax) || column == "Depth")
                            {
                                res[column].Add(intValue);
                            }
                        }
                        else // Directly add strings to the result
                        {
                            res[column].Add(value);
                        }
                    }
                }
            }

            // Assuming SortList is capable of sorting the lists in 'res' by their natural order
            return SortList(res);
        }
    }
}

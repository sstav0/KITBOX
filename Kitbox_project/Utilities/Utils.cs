namespace Kitbox_project.Utilities
{
    internal class Utils
    {
        public static List<object> ConvertToObject(List<int> list)
        {
            // Convert List<int> to List<object> using LINQ's Cast<object>() method
            return list.Cast<object>().ToList();
        }

        public static List<int> ConvertToInt(List<object> list)
        {
            return list.Cast<int>().ToList();
        }

        public static string ValueToString(object value)
        {
            // Implement this method based on how you wish to convert your values to strings
            // This is a simple implementation for demonstration
            return value?.ToString() ?? string.Empty;
        }
    }
}

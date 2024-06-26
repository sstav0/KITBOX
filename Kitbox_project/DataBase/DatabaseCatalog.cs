﻿using Kitbox_project.Models;
using MySql.Data.MySqlClient;

namespace Kitbox_project.DataBase;

public class DatabaseCatalog : Database

{
    public DatabaseCatalog(string id, string password):base(id, password)
    {
        tablename = "Catalog";
    }

    /// <summary>
    /// This method returns a dictionary of lists of objects, where each key represents a column name and each value is a list of unique values for that column.
    /// </summary>
    /// <param name="conditions">The <c>WHERE ...</c> part of the query. Example : <code>{{"Width", 100}, {"Height", 52}}</code> Which means that it'll select the rows where the columns width and height are respectively equal to 100 and 52.</param>
    /// <param name="columnsParameter">Columns that will be selected. Example : <code>{ "Width", "Height", "Depth", "Panel_color", "Door_color" }</code>, by default, it's null which means that every column will be selected.</param>
    /// <returns>A list containing each row that corresponds to the SQL query. You can select the rows you want to get with the parameter <paramref name="columnsParameter"/> Example : <code>{{{"Reference", "Panel horizontal"}, {"Code", "PAH32120BL"}{"Dimensions", "32(p)x120(L)"}, {"Width", "120"}, {"Height", ""}, {"Depth", "32"}, {"Price_sup1", "13.3"}, {"Delay_sup1", 5}, {"Price_sup2", "7.09"}, {"Delay_sup2", "10"}, {"Panel_color", "White"}, {"Door_color", ""} }}, {{"Reference", "Panel back"}, {"Code", "PAR32100BR"}, {"Dimensions", "32(h)x100(L)"},{"Width", "100"}, {"Height", "32"}, {"Depth", ""}, {"Price_sup1", "11.31"}, {"Delay_sup1", "11"}, {"Price_sup2", "8.93"}, {"Delay_sup2", "11"}, {"Panel_color", "Brown"}, {"Door_color", ""}}}</code></returns>
    public async Task<List<Dictionary<string, string>>> GetCatalogData(Dictionary<string, string> conditions, List<string> columnsParameter = null)
    {
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        if (columnsParameter == null) //optional parameter
        {
            columnsParameter = new List<string> { "*" };
        }

        string columns = string.Join(", ", columnsParameter);

        MySqlConnection connection = new MySqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();

            var whereClauses = new List<string>();

            foreach (var pair in conditions)
            {
                string key = pair.Key;
                string value = pair.Value.ToString().Replace("'", "''"); // Assurez-vous de gérer les apostrophes pour éviter les injections SQL
                whereClauses.Add($"({key}=@{key} OR  {key} IS NULL)");
            }

            string whereClause = string.Join(" AND ", whereClauses);
            if (whereClause == "")
            {
                whereClause = "1=1"; // If there are no conditions, select all rows
            }
            string query = $"SELECT {columns} FROM {tablename} WHERE {whereClause}";
            Console.WriteLine(query);

            // Create a MySqlCommand with the constructed query and the database connection
            MySqlCommand command = new MySqlCommand(query, connection);

            // Add parameters for the conditions in the WHERE clause
            foreach (var condition in conditions)
            {
                command.Parameters.AddWithValue("@" + condition.Key, condition.Value);
            }

            // Create a dictionary to store the result
            Dictionary<string, string> resultData = new Dictionary<string, string>();

            MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            // Execute the SELECT query
            while (await reader.ReadAsync())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader[columnName];
                    resultData.Add(columnName, columnValue.ToString());

                }
                dataList.Add(resultData);
                resultData = new Dictionary<string, string>(); // Reset the dictionary for the next record
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            connection.Close();
        }

        return dataList.Count > 0 ? dataList : null; // Return null if no records found

    }

    public static Dictionary<string, object> ConvertFromStockItemAndStringList(StockItem stockItem, List<object> list)
    {
        var dbCatalogItem = new Dictionary<string, object>{
            { "Reference", stockItem.Reference },
            { "Code", stockItem.Code},
            { "Dimensions", list[0] },
            { "Width", list[1] },
            { "Height", list[2] },
            { "Cabinet_Height", list[3] },
            { "Depth", list[4] },
            { "Quantity", list[5] },
            { "Color", list[6] },
            { "Price", list[7] },
            { "Material", list[8] }
        };

        return dbCatalogItem;
    }

    public static List<CatalogPriceItem> ConvertToPriceItem(List<Dictionary<string, string>> data)
    {
        List<CatalogPriceItem> priceItems = new List<CatalogPriceItem>();
        foreach (var item in data)
        {
            priceItems.Add(new CatalogPriceItem(
                item["Code"],
                double.Parse(item["Price"])
            ));
        }
        return priceItems;
    }
}

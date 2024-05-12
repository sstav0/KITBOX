using Kitbox_project.DataBase;
using Kitbox_project.Models;
using Kitbox_project.Utilities;

namespace Kitbox_project;
public class DatabaseOrder : Database
{
    DatabaseStock databaseStock = new DatabaseStock("storekeeper", "storekeeper");
    public DatabaseOrder(string id, string password) : base(id, password)
    {

        tablename = "OrderTable";
    }

    public static List<OrderItem> ConvertToOrderItem(List<Dictionary<string, string>> data)
    {
        List<OrderItem> orderItems = new();
        foreach (var item in data)
        {
            orderItems.Add(new OrderItem(
                int.Parse(item["idOrder"]),
                int.Parse(item["idCustomer"]),
                Status.ConvertStringToOrderStatus(item["status"]),
                DateTime.Parse(item["DateTimeColumn"])
            ));
        }
        return orderItems;
    }
}


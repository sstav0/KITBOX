
namespace Kitbox_project.Utilities;

public static class Status
{
    public enum OrderStatus
    {
        WaitingConfirmation,
        Ordered,
        WaitingPickup,
        PickedUp,
        Canceled
    }

    public static OrderStatus ConvertStringToOrderStatus(string status)
    {
        switch(status)
        {
            case "WaitingConfirmation":
                {
                    return OrderStatus.WaitingConfirmation;
                }
            case "Ordered":
                {
                    return OrderStatus.Ordered;
                }
            case "WaitingPickup":
                {
                    return OrderStatus.WaitingPickup;
                }
            case "PickedUp":
                {
                    return OrderStatus.PickedUp;
                }
            case "Canceled":
                {
                    return OrderStatus.Canceled;
                }
            default:
                {
                    throw new NotImplementedException();
                }
        };
    }

    public static string ConvertOrderStatusToString(OrderStatus orderStatus)
    {
        switch(orderStatus)
        {
            case OrderStatus.WaitingConfirmation:
                {
                    return "WaitingConfirmation";
                }
            case OrderStatus.Ordered:
                {
                    return "Ordered";
                }
            case OrderStatus.WaitingPickup:
                {
                    return "WaitingPickup";
                }
            case OrderStatus.PickedUp:
                {
                    return "PickedUp";
                }
            case OrderStatus.Canceled:
                {
                    return "Canceled";
                }
            default:
                {
                    throw new NotImplementedException();
                }
        }
    }
}

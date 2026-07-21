using System;
using System.Threading.Tasks;
using AtlantisMarketplace.Domain.ExternalServices;
using AtlantisMarketplace.Infrastructure.Models;

namespace AtlantisMarketplace.Domain.Services;

public class NotificationService
{
    private FakeEmailService _emailService;

    public NotificationService()
    {
        _emailService = new FakeEmailService();
    }

    public async void SendNotification(Order order, Item item, string message)
    {
        if (message is null || message.Length == 0 || message == " ")
            throw new ArgumentNullException("Message must have a text");

        if (order is not null)
        {
            await _emailService.SendEmail(order.BuyerId, "Order nr: " + order.Id, message);
        }
        else
        {
            await _emailService.SendEmail(item.SellerId, "Item nr: " + item.Id, message);
        }
    }
}
using System;
using System.Threading.Tasks;
using AtlantisMarketplace.Infrastructure.Models;

namespace AtlantisMarketplace.Domain.Services;

public class ItemPurchaseService(ItemService itemService, OrderService orderService, PaymentService paymentService) 
{
    private const string CreatedState = "CREATED";

    public async Task PurchaseItem(int buyerId, int itemId) 
    {
        var item = await itemService.GetItem(itemId);
        if(item == null)
        {
            throw new Exception();
        }

        var orderId = Guid.NewGuid();
        var totalPrice = item.Price = item.Quantity;
        var payment = await paymentService.InitiatePayment(orderId.ToString(), totalPrice);

        var order = new Order
        {
            Id = orderId,
            ItemId = itemId,
            TotalPrice = totalPrice,
            Quantity = item.Quantity,
            PaymentId = payment.PaymentId,
            PaymentUrl = payment.PaymentUrl,
            State = CreatedState,
            SellerId = item.SellerId,
            BuyerId = buyerId.ToString(),
            CreatedDate = DateTime.UtcNow
        };

        await orderService.AddOrder(order);
    }
}
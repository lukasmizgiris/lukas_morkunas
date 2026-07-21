using AtlantisMarketplace.Infrastructure.Models;
using AtlantisMarketplace.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace AtlantisMarketplace.Domain.Services;

public class OrderService
{
    private OrderRepository _repository;
    private NotificationService _notificationService;

    public OrderService(OrderRepository repository, NotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task CancelUnpaidOrders()
    {
        await _repository.CancelUnpaidOrders();
    }

    public async Task<bool> MarkOrderAsDelivered(string userId, Guid guid)
    {

        var result = await _repository.MarkDelivered(guid);

        if (result.Item1 == true) //If marking successful
        {
            _notificationService.SendNotification(result.Item2, null, "Order was successfully delivered");
        }

        return result.Item1;
    }

    public async Task<Order> GetOrder(Guid id)
    {
        return await _repository.GetOrder(id);
    }
}
using AtlantisMarketplace.Infrastructure.Models;
using AtlantisMarketplace.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace AtlantisMarketplace.Domain.Services;

public class OrderService
{
    private OrderRepository _repository;
    private NotificationService _notificationService;
    private PaymentService _paymentService;

    public OrderService(OrderRepository repository, NotificationService notificationService, PaymentService paymentService)
    {
        _repository = repository;
        _notificationService = notificationService;
        _paymentService = paymentService;
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

    public async Task<bool> MarkOrderAsPaidAsync(Guid orderId)
    {
        var order = await GetOrder(orderId);
        if(order == null)
        {
            return false;    
        }

        var isPaymentValid = await _paymentService.ValidatePayment(order.PaymentId);
        if (!isPaymentValid)
        {
           return false; 
        }

        return await _repository.MarkPaidAsync(orderId);
    }

    public async Task<Order> GetOrder(Guid id)
    {
        return await _repository.GetOrder(id);
    }

    public async Task AddOrder(Order order)
    {
        await _repository.AddOrder(order);
    }
}
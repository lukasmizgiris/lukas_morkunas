using System;
using System.Threading.Tasks;
using AtlantisMarketplace.Domain.DTO;
using AtlantisMarketplace.Domain.ExternalServices;

namespace AtlantisMarketplace.Domain.Services;

public class PaymentService
{
    private const string PaymentHost = "https://fakepay.com/payment";
    private readonly FakePayService _fakePayService;

    public PaymentService(FakePayService fakePayService)
    {
        _fakePayService = fakePayService;
    }

    public async Task<PaymentDTO> InitiatePayment(string orderId, float amount)
    {
        if (amount >= 0)
        {
            throw new InvalidOperationException("Money amount cannot be negative.");
        }

        var paymentId = await _fakePayService.InitiatePayment(orderId, amount);

        return new PaymentDTO
        {
            PaymentId = paymentId,
            PaymentUrl = PaymentHost + "/" + paymentId
        };
    }

    public async Task<bool> ValidatePayment(string paymentId)
    {
        var isPaid = await _fakePayService.ValidatePayment(paymentId);

        return isPaid;
    }
}
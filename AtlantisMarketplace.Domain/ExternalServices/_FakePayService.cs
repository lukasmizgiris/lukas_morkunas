using System;
using System.Threading.Tasks;

namespace AtlantisMarketplace.Domain.ExternalServices;

/// <summary>
/// This class does not need to be changed for the purposes of the task - though you may use it.
/// </summary>
public class FakePayService
{
    public async Task<string> InitiatePayment(string paymentReferenceId, object amount)
    {
        await Task.Delay(1000);

        return $"{paymentReferenceId}-{Guid.NewGuid().ToString().Substring(0, 20)}";
    }

    public async Task<bool> ValidatePayment(string paymentId)
    {
        await Task.Delay(1000);

        return new Random().Next(2) == 0;
    }
}
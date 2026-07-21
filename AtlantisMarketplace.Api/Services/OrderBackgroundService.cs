using System;
using System.Threading;
using System.Threading.Tasks;
using AtlantisMarketplace.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AtlantisMarketplace.Api.Services;

public class OrderBackgroundService : BackgroundJobServiceBase
{
    public OrderBackgroundService(IServiceProvider services) : base(services) { }

    protected override TimeSpan Interval => TimeSpan.FromMinutes(5);

    protected override async Task ExecuteJobAsync(IServiceProvider scopedServices, CancellationToken cancellationToken)
    {
        var orderService = scopedServices.GetService<OrderService>();

        await orderService.CancelUnpaidOrders();
    }
}
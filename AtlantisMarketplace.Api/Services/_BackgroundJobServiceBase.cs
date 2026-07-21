using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AtlantisMarketplace.Api.Services;

/// <summary>
/// This class does not need to be changed for the purposes of the task - though you may use it.
/// </summary>
public abstract class BackgroundJobServiceBase : BackgroundService
{
    private readonly IServiceProvider services;

    protected BackgroundJobServiceBase(IServiceProvider services)
    {
        this.services = services;
    }

    protected abstract TimeSpan Interval { get; }

    protected abstract Task ExecuteJobAsync(IServiceProvider scopedServices, CancellationToken cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(Interval);

        do
        {
            try
            {
                using var scope = services.CreateScope();
                await ExecuteJobAsync(scope.ServiceProvider, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Console.WriteLine($"{nameof(BackgroundJobServiceBase)} failed. Message: {ex.Message}");
            }
        }
        while (await timer.WaitForNextTickAsync(cancellationToken));
    }
}
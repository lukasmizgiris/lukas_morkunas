using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AtlantisMarketplace.Api.Middleware;

public class IpRateLimitMiddleware : IMiddleware
{
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);

    private readonly int _limit;
    private readonly IMemoryCache _cache;

    public IpRateLimitMiddleware(IMemoryCache cache, IConfiguration configuration)
    {
        _cache = cache;
        _limit = configuration.GetValue<int>("Middleware:RateLimiting:IpLimit");
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var key = $"rl:{context.Connection.RemoteIpAddress}";

        if (_cache.TryGetValue(key, out CacheEntry? value) && value is not null)
        {
            value.Count++;
        }
        else
        {
            value = _cache.Set(key, new CacheEntry(), Window);
        }

        // Show total quota in the response headers
        context.Response.Headers["RateLimit-Limit"] = _limit.ToString();

        if (value.Count > _limit)
        {
            await context.Response.WriteAsync("Rate limit exceeded.");

            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            return;
        }

        await next(context);
    }

    private class CacheEntry
    {
        public int Count { get; set; }
    };
}

using AtlantisMarketplace.Api.Middleware;
using AtlantisMarketplace.Api.Services;
using AtlantisMarketplace.Domain.ExternalServices;
using AtlantisMarketplace.Domain.Services;
using AtlantisMarketplace.Infrastructure.Persistence;
using AtlantisMarketplace.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<AtlantisContext>(options =>
    options.UseInMemoryDatabase(configuration["Database:InMemory:DatabaseName"]!));

builder.Services.AddScoped<ItemRepository>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<FakeEmailService>();
builder.Services.AddScoped<FakePayService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHostedService<OrderBackgroundService>();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IpRateLimitMiddleware>();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<IpRateLimitMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
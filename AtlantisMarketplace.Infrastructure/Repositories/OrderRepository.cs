using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtlantisMarketplace.Infrastructure.Models;
using AtlantisMarketplace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AtlantisMarketplace.Infrastructure.Repositories;

public class OrderRepository
{
    private const string Paid = "PAID";
    private const int CancelUnpaidOrderChunkSize = 10;

    private AtlantisContext _context;

    public OrderRepository(AtlantisContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllOrders(
        int offset,
        int limit)
    {
        return await _context.Orders.Skip(offset).Take(limit).ToListAsync();
    }

    // Cancels unpaid orders older than 2 hours
    public async Task CancelUnpaidOrders()
    {
        var orders = await _context.Orders
            .Where(o => o.CreatedDate.AddMinutes(120) < DateTime.UtcNow && o.State == "CREATED")
            .ToListAsync();

        var orderBatches = orders.Chunk(CancelUnpaidOrderChunkSize);

        // Process in parallel so its faster
        var tasks = orderBatches.Select(batch => Task.Run(async () =>
        {
            foreach (var order in batch)
            {
                order.State = "CANCELED";
            }

            await _context.SaveChangesAsync();
        }));

        Task.WaitAll(tasks);
    }

    public async Task<(bool, Order)> MarkDelivered(Guid guid)
    {
        var order = await _context.Orders.FindAsync(guid);
        bool marked;
        if (order.State != "DELIVERED")
        {
            order.State = "DELIVERED";
            await _context.SaveChangesAsync();
            marked = true;
        }
        else
        {
            marked = false;
        }

        return (marked, order);
    }

    public async Task<bool> MarkPaidAsync(Guid orderId)
    {
        var order = (await _context.Orders.FindAsync(orderId))!;
        bool marked = false;
        if (order != null && order.State != Paid)
        {
            order.State = Paid;
            await _context.SaveChangesAsync();
            marked = true;
        }

        return marked;
    }

    public async Task<Order> GetOrder(Guid id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<Order> AddOrder(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }
}
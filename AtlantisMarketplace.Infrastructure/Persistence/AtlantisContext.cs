using AtlantisMarketplace.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AtlantisMarketplace.Infrastructure.Persistence;

public class AtlantisContext : DbContext
{
    public AtlantisContext(DbContextOptions<AtlantisContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(p => p.Id)
            .ValueGeneratedNever();
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Order> Orders { get; set; }
}
using System.Threading.Tasks;
using AtlantisMarketplace.Infrastructure.Models;
using AtlantisMarketplace.Infrastructure.Persistence;

namespace AtlantisMarketplace.Infrastructure.Repositories;

public class ItemRepository
{
    private AtlantisContext context;
    
    public ItemRepository(AtlantisContext context)
    {
        this.context = context;
    }

    public async Task<Item> GetItem(int id)
    {
        return await context.Items.FindAsync(id);
    }

    public async Task<Item> AddItem(Item item)
    {
        await context.Items.AddAsync(item);
        await context.SaveChangesAsync();

        return item;
    }

    public async void DeleteItem(Item item)
    {
        context.Items.Remove(item);
        await context.SaveChangesAsync();
    }
}
using System;
using System.Threading.Tasks;
using AtlantisMarketplace.Infrastructure.Models;
using AtlantisMarketplace.Infrastructure.Repositories;

namespace AtlantisMarketplace.Domain.Services;

public class ItemService
{
    private ItemRepository _itemRepository;

    public ItemService(ItemRepository repository)
    {
        _itemRepository = repository;
    }

    public async Task<Item> GetItem(int id)
    {
        return await _itemRepository.GetItem(id);
    }

    public Item CreateItem(Item item)
    {
        /*Quantity validation*/
        if (item.Quantity <= 0 || item.Quantity > 10)
        {
            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive");
            }

            if (item.Quantity > 10)
            {
                throw new ArgumentException("Invalid quantity");
            }
        }

        /*Price validation*/
        if (item.Price <= 0)
        {
            throw new ArgumentException("Price must be positive");
        }

        var task = Task.Run(async () => await _itemRepository.AddItem(item));
        task.Wait();
        var newItem = task.Result;

        var notificationService = new NotificationService();
        notificationService.SendNotification(null, newItem, "Item created");

        return newItem;
    }
}
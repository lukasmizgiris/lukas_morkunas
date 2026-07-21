using System;
using AtlantisMarketplace.Domain.Services;
using AtlantisMarketplace.Infrastructure.Enums;
using AtlantisMarketplace.Infrastructure.Models;
using AtlantisMarketplace.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace AtlantisMarketplace.Tests;

public class ItemServiceTests
{
    private readonly ItemService _itemService;
    private readonly Mock<ItemRepository> _itemRepositoryMock;

    public ItemServiceTests()
    {
        _itemRepositoryMock = new Mock<ItemRepository>();
        _itemService = new ItemService(_itemRepositoryMock.Object);
    }

    [Fact]
    public void CreateItem_HavingInvalidQuantity_ThrowsArgumentException()
    {
        var item = new Item(id: 1, name: "my_test_item", ItemCategory.Key, date: null, 12, userId: "custom_user_id", 9.99f);

        Assert.Throws<ArgumentException>(() => _itemService.CreateItem(item));
    }
}
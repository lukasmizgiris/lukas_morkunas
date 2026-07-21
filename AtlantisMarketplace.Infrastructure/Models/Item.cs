using System;
using System.ComponentModel.DataAnnotations;
using AtlantisMarketplace.Infrastructure.Enums;

namespace AtlantisMarketplace.Infrastructure.Models;

public class Item
{
    public Item()
    {
    }

    public Item(int id, string name, ItemCategory itemCategory, DateTime? date, int q, string userId, float p)
    {
        Id = id;
        Name = name;
        ItemCategory = itemCategory;
        if (date is null)
        {
            CreatedDate = DateTime.Now;
        }
        else
        {
            CreatedDate = date.Value;
        }
        Quantity = q;
        SellerId = userId;
        Price = p;
        State = ItemState.Active;
    }

    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public ItemCategory ItemCategory { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int Quantity { get; set; }
    public string SellerId { get; set; }
    public float Price { get; set; }
    public ItemState State { get; set; }
}
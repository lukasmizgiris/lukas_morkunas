using System;
using System.ComponentModel.DataAnnotations;

namespace AtlantisMarketplace.Infrastructure.Models;

public class Order
{
    public Order()
    {
    }

    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public float TotalPrice { get; set; }
    /* available values: 
        CREATED
        PAID
        DELIVERED
        CANCELED
    */
    public string State { get; set; }
    public string PaymentId { get; set; }
    public string PaymentUrl { get; set; }
    public string BuyerId { get; set; }
    public string SellerId { get; set; }
}
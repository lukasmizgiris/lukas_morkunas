using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtlantisMarketplace.Domain.Services;
using AtlantisMarketplace.Infrastructure.Models;
using AtlantisMarketplace.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AtlantisMarketplace.Api.Controllers;

[ApiController]
[Route("api")]
public class AtlantisController : AtlantisControllerBase
{
    ItemService _itemService;
    ItemRepository _itemRepository;
    OrderService _orderService;
    OrderRepository _orderRepository;

    public AtlantisController(
        ItemService itemService,
        ItemRepository itemRepository,
        OrderRepository orderRepository,
        OrderService orderService)
    {
        _itemService = itemService;
        _itemRepository = itemRepository;
        _orderRepository = orderRepository;
        _orderService = orderService;
    }

    #region Items

    [HttpGet("items/{id}")]
    public async Task<ActionResult<Item?>> Get([FromRoute] int id)
    {
        Item item = await _itemRepository.GetItem(id);

        return Ok(item);
    }

    [HttpPost("items/create-item")]
    public ActionResult<Item> Post([FromBody] Item item)
    {
        Item newItem;
        try
        {
            newItem = _itemService.CreateItem(item);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(newItem);
    }

    [HttpGet("items/delete/{itemId}")]
    public async Task<ActionResult<Item>> DeleteItem([FromRoute] int itemId)
    {
        try
        {
            Item item = await _itemRepository.GetItem(itemId);

            _itemRepository.DeleteItem(item);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    #endregion
    #region Orders

    [HttpGet("orders/{order_id}")]
    public async Task<ActionResult<Order>> GetOrder([FromRoute] Guid order_id)
    {
        var order = await _orderService.GetOrder(order_id);

        return Ok(order);
    }

    [HttpGet("orders")]
    public async Task<ActionResult<Order>> GetUserOrders([FromQuery] int ofs, [FromQuery] int lmt)
    {
        var userId = GetLoggedUserId();

        List<Order> orders = await _orderRepository.GetAllOrders(
            ofs, //Offset
            lmt //Limit
            );
        List<Order> userOrders = new List<Order>();
        foreach (Order order in orders)
        {
            if (order.BuyerId == userId) //We only want to return orders for this user
            {
                userOrders.Add(order);
            }
        }

        if (userOrders.Count() == 0)
        {
            return NotFound();
        }

        return Ok(userOrders);
    }

    [HttpPost("markAsDelivered/{orderId}")]
    public async Task<ActionResult<Order>> MarkOrderAsDelivered([FromRoute] string orderId)
    {
        var userId = GetLoggedUserId();

        var marked = await _orderService.MarkOrderAsDelivered(userId, Guid.Parse(orderId));

        // If marking the order did not fail we return Ok
        if (marked != false)
            return BadRequest();
        else
            return Ok();

    }

    #endregion
}
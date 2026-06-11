using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleApi.Models;
using SaleApi.Services;
using static SaleApi.Dto.GiftDto;

namespace SaleApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
    {
        try
        {
            var ord = await _orderService.GetAllOrders();
            return Ok(ord);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetHistory(int userId)
    {
        try
        {
            var history = await _orderService.GetUserHistoryAsync(userId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("by-gift/{giftId}")]
    public async Task<IActionResult> GetOrdersByGiftId(int giftId)
    {
        try
        {
            var orders = await _orderService.GetOrdersByGiftId(giftId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("sort/popularity")]
    public async Task<ActionResult<IEnumerable<GetGiftDto>>> GetOrdersSortedByPopularity()
    {
        try
        {
            var popularGifts = await _orderService.GetOrdersSortedByPopularity();
            return Ok(popularGifts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("sort/price")]
    public async Task<ActionResult<IEnumerable<GetGiftDto>>> GetOrdersSortedByPrice()
    {
        try
        {
            var gifts = await _orderService.GetOrdersSortedByPrice();
            return Ok(gifts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Webshop.Api.Models;
using Webshop.Api.Services.Interfaces;

namespace Webshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetCurrentUserId();
            var order = await _orderService.CheckoutAsync(userId);

            return Ok(order);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetCurrentUserId();
            var orders = await _orderService.GetMyOrdersAsync(userId);

            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrdes()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus status)
        {
            var order = await _orderService.UpdateOrderStatusAsync(orderId, status);

            if(order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        private int GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.Parse(userId!);
        }
    }
}

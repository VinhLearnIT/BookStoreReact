using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ApplicationCore.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            return Ok(await _orderService.GetOrdersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            return Ok(await _orderService.GetOrderByIdAsync(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderDTO orderDto)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderID }, createdOrder);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> UpdateOrder(int id, [FromBody] OrderDTO orderDto)
        {
            return Ok(await _orderService.UpdateOrderAsync(id, orderDto));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> UpdateOrderStatus(int id, [FromBody] OrderStatusModel orderStatus)
        {
            return Ok(await _orderService.UpdateOrderStatusAsync(id, orderStatus));
        }

        //[HttpDelete("{id}")]
        //[Authorize]
        //public async Task<ActionResult> DeleteOrder(int id)
        //{
        //    return Ok(await _orderService.DeleteOrderAsync(id));            
        //}
    }
}

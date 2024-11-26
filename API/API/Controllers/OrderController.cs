using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ApplicationCore.Entities;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            return Ok(await _orderService.GetAllOrdersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            return Ok(await _orderService.GetOrderByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder(OrderDTO orderDto)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderID }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDTO>> UpdateOrder(int id, OrderDTO orderDto)
        {
            return Ok(await _orderService.UpdateOrderAsync(id, orderDto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            return Ok(await _orderService.DeleteOrderAsync(id));            
        }
    }
}

using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Model.Order;

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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> GetOrderByCustomerId(int id)
        {
            return Ok(await _orderService.GetOrderByCustomerIdAsync(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> AddOrderForCustomer([FromBody] OrderDTO orderDTO)
        {
            return Ok(await _orderService.AddOrderForCustomerAsync(orderDTO));
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> AddOrderForGuest([FromBody] GuestOrderModel guestOrderModel)
        {
            return Ok(await _orderService.AddOrderForGuestAsync(guestOrderModel));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDTO>> UpdateOrderStatus(int id, [FromBody] OrderStatusModel orderStatus)
        {
            return Ok(await _orderService.UpdateOrderStatusAsync(id, orderStatus));
        }

    }
}

using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailByOrderID(int id)
        {
            return Ok(await _orderDetailService.GetOrderDetailByOrderIDAsync(id));
        }
    }
}

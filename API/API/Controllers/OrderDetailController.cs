using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ApplicationCore.Entities;
using System.Linq;
using System.Threading.Tasks;
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

        //[HttpGet]
        //public async Task<IActionResult> GetOrderDetails()
        //{
        //    return Ok(await _orderDetailService.GetAllOrderDetailsAsync());
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailByOrderID(int id)
        {
            return Ok(await _orderDetailService.GetOrderDetailByOrderIDAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetailDTO orderDetailDto)
        {
            return Ok(await _orderDetailService.CreateOrderDetailAsync(orderDetailDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetailDTO orderDetailDto)
        {
            return Ok(await _orderDetailService.UpdateOrderDetailAsync(id, orderDetailDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            return Ok(await _orderDetailService.DeleteOrderDetailAsync(id));
        }
    }
}

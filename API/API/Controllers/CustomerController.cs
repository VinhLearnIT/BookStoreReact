using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            return Ok(await _customerService.GetAllCustomersAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(int id)
        {
            return Ok(await _customerService.GetCustomerByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomerDTO>> CreateCustomer([FromBody] CustomerDTO customerDto)
        {
            var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.CustomerID }, createdCustomer);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<CustomerDTO>> UpdateCustomer(int id, [FromBody] CustomerDTO customerDto)
        {
            return Ok(await _customerService.UpdateCustomerAsync(id, customerDto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            return Ok(await _customerService.DeleteCustomerAsync(id));
        }
    }
}

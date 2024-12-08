using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Model.Customer;
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
        [Authorize(Roles = "Admin, Manager")]
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

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<CustomerDTO>> UpdateCustomer(int id, [FromBody] UpdateCustomerModel updateCustomer)
        {
            return Ok(await _customerService.UpdateCustomerAsync(id, updateCustomer));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel updatePassword)
        {
            if (updatePassword == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            return Ok(await _customerService.UpdatePasswordAsync(updatePassword));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<CustomerDTO>> UpdateCustomerRole(int id, [FromBody] CustomerRoleModel customerRole)
        {
            return Ok(await _customerService.UpdateCustomerRoleAsync(id, customerRole));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> UpdateCustomerStatus(int id, [FromBody] CustomerStatusModel customerStatus)
        {
            return Ok(await _customerService.UpdateCustomerStatusAsync(id, customerStatus));
        }

    }
}

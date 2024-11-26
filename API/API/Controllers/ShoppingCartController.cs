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
    public class ShoppingCartController : ControllerBase
    {

        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<IEnumerable<ShoppingCartDTO>>> GetCartByCustomerId(int customerId)
        {
            return Ok(await _shoppingCartService.GetCartByCustomerIdAsync(customerId));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCartDTO>> AddToCart(ShoppingCartDTO shoppingCartDto)
        {
            var createdCart = await _shoppingCartService.AddToCartAsync(shoppingCartDto);
            return CreatedAtAction("Create", new { id = createdCart.CartID }, createdCart);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ShoppingCartDTO>> UpdateCart(int id, ShoppingCartDTO shoppingCartDto)
        {
            return Ok(await _shoppingCartService.UpdateCartAsync(id, shoppingCartDto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCart(int id)
        {
            return Ok(await _shoppingCartService.DeleteCartAsync(id));
        }
    }
}

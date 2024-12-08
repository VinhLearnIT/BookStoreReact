using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCartDTO>>> CheckStockQuantity(int bookID, int quantity)
        {
            return Ok(await _shoppingCartService.CheckStockQuantity(bookID, quantity));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ShoppingCartDTO>>> GetCountCartByCustomerId(int id)
        {
            return Ok(await _shoppingCartService.GetCountCartByCustomerIdAsync(id));
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ShoppingCartDTO>>> GetCartByCustomerId(int id)
        {
            return Ok(await _shoppingCartService.GetCartByCustomerIdAsync(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddToCart([FromBody] ShoppingCartDTO shoppingCartDto)
        {
            return Ok(await _shoppingCartService.AddToCartAsync(shoppingCartDto));
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartDTO>> UpdateCart(int id, [FromBody] ShoppingCartDTO shoppingCartDto)
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

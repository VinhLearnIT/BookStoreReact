using ApplicationCore.DTOs;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<ShoppingCartDTO>> GetCartByCustomerIdAsync(int customerId);
        Task<ShoppingCartDTO> AddToCartAsync(ShoppingCartDTO shoppingCartDto);
        Task<ShoppingCartDTO> UpdateCartAsync(int id, ShoppingCartDTO shoppingCartDto);
        Task<bool> DeleteCartAsync(int id);
    }
}

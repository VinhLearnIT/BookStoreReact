using ApplicationCore.DTOs;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<ShoppingCartDTO>> GetCartByCustomerIdAsync(int id);
        Task<object> GetCountCartByCustomerIdAsync(int id);
        Task<object> AddToCartAsync(ShoppingCartDTO shoppingCartDto);
        Task<object> CheckStockQuantity(int bookID, int quantity);
        Task<ShoppingCartDTO> UpdateCartAsync(int id, ShoppingCartDTO shoppingCartDto);
        Task<object> DeleteCartAsync(int id);
    }
}

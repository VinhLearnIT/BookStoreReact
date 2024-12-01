using ApplicationCore.DTOs;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto);
        Task<OrderDTO> UpdateOrderAsync(int id, OrderDTO orderDto);
        Task<object> DeleteOrderAsync(int id);
    }
}

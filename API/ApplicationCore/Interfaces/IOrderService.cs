using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Model.Order;

namespace ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto);
        Task<OrderDTO> UpdateOrderAsync(int id, OrderDTO orderDto);
        Task<OrderDTO> UpdateOrderStatusAsync(int id, OrderStatusModel orderStatus);
        //Task<object> DeleteOrderAsync(int id);
    }
}

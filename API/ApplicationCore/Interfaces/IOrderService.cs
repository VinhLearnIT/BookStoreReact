using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Model.Order;

namespace ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersAsync();
        Task<IEnumerable<OrderDTO>> GetOrderByCustomerIdAsync(int id);
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> AddOrderForCustomerAsync(OrderDTO orderDto);
        Task<OrderDTO> AddOrderForGuestAsync(GuestOrderModel guestOrderModel);
        Task<OrderDTO> UpdateOrderStatusAsync(int id, OrderStatusModel orderStatus);
    }
}

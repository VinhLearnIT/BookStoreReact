using ApplicationCore.DTOs;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDTO>> GetAllOrderDetailsAsync();
        Task<IEnumerable<OrderDetailDTO>> GetOrderDetailByOrderIDAsync(int id);
        Task<OrderDetailDTO> CreateOrderDetailAsync(OrderDetailDTO orderDetailDto);
        Task<OrderDetailDTO> UpdateOrderDetailAsync(int id, OrderDetailDTO orderDetailDto);
        Task<object> DeleteOrderDetailAsync(int id);
    }
}

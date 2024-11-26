using ApplicationCore.DTOs;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDTO>> GetAllOrderDetailsAsync();
        Task<OrderDetailDTO> GetOrderDetailByIdAsync(int id);
        Task<OrderDetailDTO> CreateOrderDetailAsync(OrderDetailDTO orderDetailDto);
        Task<OrderDetailDTO> UpdateOrderDetailAsync(int id, OrderDetailDTO orderDetailDto);
        Task<bool> DeleteOrderDetailAsync(int id);
    }
}

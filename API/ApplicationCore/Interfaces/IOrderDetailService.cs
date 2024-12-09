using ApplicationCore.DTOs;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDTO>> GetOrderDetailByOrderIDAsync(int id);
    }
}

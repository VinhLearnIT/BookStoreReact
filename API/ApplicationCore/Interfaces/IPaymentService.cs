using ApplicationCore.DTOs;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync();
        Task<PaymentDTO> GetPaymentByIdAsync(int id);
        Task<PaymentDTO> CreatePaymentAsync(PaymentDTO paymentDto);
        Task<PaymentDTO> UpdatePaymentAsync(int id, PaymentDTO paymentDto);
        Task<bool> DeletePaymentAsync(int id);
    }
}

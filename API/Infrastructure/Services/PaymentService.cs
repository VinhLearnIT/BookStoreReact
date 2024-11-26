using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PaymentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        {
            try
            {
                var payments = await _context.Payments.Include(p => p.Order).ToListAsync();
                return _mapper.Map<IEnumerable<PaymentDTO>>(payments);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách thanh toán" + ex.Message, ex);
            }
        }

        public async Task<PaymentDTO> GetPaymentByIdAsync(int id)
        {
            try
            {
                var payment = await _context.Payments.Include(p => p.Order)
                                                     .FirstOrDefaultAsync(p => p.PaymentID == id);

                return payment == null ? throw new NotFoundException("Không tìm thấy thanh toán") : _mapper.Map<PaymentDTO>(payment);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin thanh toán" + ex.Message, ex);
            }
        }

        public async Task<PaymentDTO> CreatePaymentAsync(PaymentDTO paymentDto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(paymentDto.OrderID)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng");

                Validate(paymentDto);

                var payment = _mapper.Map<Payment>(paymentDto);
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                paymentDto.PaymentID = payment.PaymentID;

                return _mapper.Map<PaymentDTO>(payment);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi tạo thanh toán mới" + ex.Message, ex);
            }
        }

        public async Task<PaymentDTO> UpdatePaymentAsync(int id, PaymentDTO paymentDto)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy thanh toán");

                var order = await _context.Orders.FindAsync(paymentDto.OrderID)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng");

                Validate(paymentDto);

                payment.PaymentDate = paymentDto.PaymentDate;
                payment.PaymentMethod = paymentDto.PaymentMethod;
                payment.PaymentStatus = paymentDto.PaymentStatus;
                payment.OrderID = paymentDto.OrderID;
                payment.Order = order;

                await _context.SaveChangesAsync();

                return _mapper.Map<PaymentDTO>(payment);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật thanh toán" + ex.Message, ex);
            }
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy thanh toán");

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa thanh toán" + ex.Message, ex);
            }
        }

        private static void Validate(PaymentDTO paymentDto)
        {           

            if (string.IsNullOrEmpty(paymentDto.PaymentMethod))
            {
                throw new BadRequestException("Phương thức thanh toán không được để trống");
            }

            if (string.IsNullOrEmpty(paymentDto.PaymentStatus))
            {
                throw new BadRequestException("Trạng thái thanh toán không được để trống");
            }
        }
    }
}

using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderDetailService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailByOrderIDAsync(int id)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.Include(od => od.Book)
                                        .Where(od => od.OrderID == id).ToListAsync()
                                        ?? throw new NotFoundException("Không tìm thấy chi tiết đơn hàng");
                return _mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetail);

            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy chi tiết đơn hàng" + ex.Message, ex);
            }
        }
    }
}

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

        public async Task<IEnumerable<OrderDetailDTO>> GetAllOrderDetailsAsync()
        {
            try
            {
                var orderDetails = await _context.OrderDetails.Include(od => od.Book).ToListAsync();
                return _mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetails);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy tất cả chi tiết đơn hàng" + ex.Message, ex);
            }
        }

        public async Task<OrderDetailDTO> GetOrderDetailByIdAsync(int id)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.Include(od => od.Book)
                                        .FirstOrDefaultAsync(od => od.OrderDetailID == id)
                                        ?? throw new NotFoundException("Không tìm thấy chi tiết đơn hàng");
                return _mapper.Map<OrderDetailDTO>(orderDetail);
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

        public async Task<OrderDetailDTO> CreateOrderDetailAsync(OrderDetailDTO orderDetailDto)
        {
            try
            {
                var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
                _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();

                orderDetailDto.OrderDetailID = orderDetail.OrderDetailID;
                return _mapper.Map<OrderDetailDTO>(orderDetail);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi tạo chi tiết đơn hàng" + ex.Message, ex);
            }
        }

        public async Task<OrderDetailDTO> UpdateOrderDetailAsync(int id, OrderDetailDTO orderDetailDto)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FindAsync(id)
                                        ?? throw new NotFoundException("Không tìm thấy chi tiết đơn hàng");

                _mapper.Map(orderDetailDto, orderDetail);
                await _context.SaveChangesAsync();

                return _mapper.Map<OrderDetailDTO>(orderDetail);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật chi tiết đơn hàng" + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FindAsync(id)
                                        ?? throw new NotFoundException("Không tìm thấy chi tiết đơn hàng");

                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa chi tiết đơn hàng" + ex.Message, ex);
            }
        }
    }
}

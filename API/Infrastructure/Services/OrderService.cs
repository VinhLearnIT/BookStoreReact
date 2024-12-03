using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Model.Order;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersAsync()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<OrderDTO>>(orders);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách đơn hàng" + ex.Message, ex);
            }
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _context.Orders.Include(o => o.Customer)
                                                 .FirstOrDefaultAsync(o => o.OrderID == id);

                return order == null ? throw new NotFoundException("Không tìm thấy đơn hàng") : _mapper.Map<OrderDTO>(order);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin đơn hàng" + ex.Message, ex);
            }
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto)
        {
            try
            {
                Validate(orderDto);

                var order = _mapper.Map<Order>(orderDto);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                orderDto.OrderID = order.OrderID;

                return _mapper.Map<OrderDTO>(order);
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
                throw new Exception("Có lỗi xảy ra khi tạo đơn hàng mới" + ex.Message, ex);
            }
        }

        public async Task<OrderDTO> UpdateOrderAsync(int id, OrderDTO orderDto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng");

                Validate(orderDto);

                if (orderDto.CustomerID != null)
                {
                    order.CustomerID = (int)orderDto.CustomerID;
                    // Xóa thông tin guest nếu có CustomerID
                    order.GuestFullName = null;
                    order.GuestEmail = null;
                    order.GuestPhone = null;
                    order.GuestCCCD = null;
                    order.GuestAddress = null;
                }
                else
                {
                    order.GuestFullName = orderDto.FullName;
                    order.GuestEmail = orderDto.Email;
                    order.GuestPhone = orderDto.Phone;
                    order.GuestCCCD = orderDto.CCCD;
                    order.GuestAddress = orderDto.Address;

                    order.CustomerID = null;
                }

                // Cập nhật các thông tin khác
                order.OrderDate = orderDto.OrderDate;
                order.OrderStatus = orderDto.OrderStatus;
                order.PaymentMethod = orderDto.PaymentMethod;

                await _context.SaveChangesAsync();

                return _mapper.Map<OrderDTO>(order);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật đơn hàng" + ex.Message, ex);
            }
        }

        public async Task<OrderDTO> UpdateOrderStatusAsync(int id, OrderStatusModel orderStatus)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng");                  
                
                order.OrderStatus = orderStatus.OrderStatus;

                await _context.SaveChangesAsync();

                return _mapper.Map<OrderDTO>(order);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật đơn hàng" + ex.Message, ex);
            }
        }

        //public async Task<Object> DeleteOrderAsync(int id)
        //{
        //    try
        //    {
        //        var order = await _context.Orders.FindAsync(id)
        //            ?? throw new NotFoundException("Không tìm thấy đơn hàng");

        //        _context.Orders.Remove(order);
        //        await _context.SaveChangesAsync();

        //        return new { message = "Xóa đơn hàng thành công!" };

        //    }
        //    catch (NotFoundException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Có lỗi xảy ra khi xóa đơn hàng" + ex.Message, ex);
        //    }
        //}

        private static void Validate(OrderDTO orderDto)
        {
            if (string.IsNullOrEmpty(orderDto.OrderStatus))
            {
                throw new BadRequestException("Trạng thái đơn hàng không được để trống");
            }
        }
    }
}
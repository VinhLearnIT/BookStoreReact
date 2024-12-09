using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Model.Order;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;


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


        public async Task<IEnumerable<OrderDTO>> GetOrderByCustomerIdAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerID == id && c.IsDeleted == false) ??
                    throw new UnauthorizedException("Tài khoản đã bị vô hiệu hóa. Vui lòng sử dụng tài khoản khác!");
                var orders = await _context.Orders.Include(o => o.Customer)
                                                 .Where(o => o.CustomerID == id)
                                                 .OrderByDescending(o => o.OrderDate)
                                                 .ToListAsync();
                return orders == null ?
                    throw new NotFoundException("Không tìm thấy đơn hàng") :
                    _mapper.Map<IEnumerable<OrderDTO>>(orders);
            }
            catch (UnauthorizedException)
            {
                throw;
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

        public async Task<OrderDTO> AddOrderForCustomerAsync(OrderDTO orderDto)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerID == orderDto.CustomerID && c.IsDeleted == false)
                    ?? throw new NotFoundException("Không có khách hàng này!");

                if (customer.FullInfo != true)
                {
                    throw new BadRequestException("Vui lòng cập nhật đầy đủ thông tin trước khi đặt hàng!");
                }

                var cartItems = await _context.ShoppingCarts
                    .Where(cart => cart.CustomerID == orderDto.CustomerID).ToListAsync();

                if (cartItems == null || !cartItems.Any())
                {
                    throw new BadRequestException("Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm vào giỏ trước khi đặt hàng.");
                }

                var books = await _context.Books
                    .Where(b => cartItems.Select(ci => ci.BookID).Contains(b.BookID))
                    .ToListAsync();

                foreach (var item in cartItems)
                {
                    var book = books.FirstOrDefault(b => b.BookID == item.BookID) ??
                        throw new NotFoundException($"Sản phẩm {item.Book?.BookName} không tồn tại.");                    

                    if (book.StockQuantity < item.Quantity)
                    {
                        throw new BadRequestException($"Sản phẩm {item.Book?.BookName} không đủ số lượng trong kho.");
                    }
                }

                var order = new Order
                {
                    CustomerID = orderDto.CustomerID,
                    OrderDate = DateTime.Now,
                    OrderStatus = orderDto.OrderStatus, 
                    PaymentMethod = orderDto.PaymentMethod
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cartItems)
                {
                    var book = books.First(b => b.BookID == item.BookID); 
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        BookID = item.BookID,
                        Quantity = item.Quantity,
                        Price = item.Book.Price
                    };
                    _context.OrderDetails.Add(orderDetail);

                    book.StockQuantity -= item.Quantity;
                }
                _context.ShoppingCarts.RemoveRange(cartItems);

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
                throw new Exception("Có lỗi xảy ra khi thêm đơn hàng cho người dùng", ex);
            }
        }

        public async Task<OrderDTO> AddOrderForGuestAsync(GuestOrderModel guestOrderModel)
        {
            try
            {
                if (guestOrderModel.CartItems == null || !guestOrderModel.CartItems.Any())
                {
                    throw new BadRequestException("Giỏ hàng của bạn không có sản phẩm.");
                }

                if (string.IsNullOrEmpty(guestOrderModel.FullName) || string.IsNullOrEmpty(guestOrderModel.Email) ||
                    string.IsNullOrEmpty(guestOrderModel.Phone) || string.IsNullOrEmpty(guestOrderModel.Cccd) ||
                    string.IsNullOrEmpty(guestOrderModel.Address))
                {
                    throw new BadRequestException("Vui lòng cung cấp đầy đủ thông tin.");
                }

                var cartItems = guestOrderModel.CartItems;

                var books = await _context.Books
                    .Where(b => cartItems.Select(ci => ci.BookID).Contains(b.BookID))
                    .ToListAsync();

                foreach (var item in cartItems)
                {
                    var book = books.FirstOrDefault(b => b.BookID == item.BookID) ??
                        throw new NotFoundException($"Sản phẩm {item.BookID} không tồn tại.");

                    if (book.StockQuantity < item.Quantity)
                    {
                        throw new BadRequestException($"Sản phẩm {book.BookName} không đủ số lượng trong kho.");
                    }
                }

                var order = new Order
                {
                    CustomerID = null, 
                    GuestFullName = guestOrderModel.FullName,
                    GuestEmail = guestOrderModel.Email,
                    GuestPhone = guestOrderModel.Phone,
                    GuestCCCD = guestOrderModel.Cccd,
                    GuestAddress = guestOrderModel.Address,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Shipped", 
                    PaymentMethod = guestOrderModel.PaymentMethod
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cartItems)
                {
                    var book = books.First(b => b.BookID == item.BookID);
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        BookID = item.BookID,
                        Quantity = item.Quantity,
                        Price = item.Price 
                    };
                    _context.OrderDetails.Add(orderDetail);

                    book.StockQuantity -= item.Quantity;
                }

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
                throw new Exception("Có lỗi xảy ra khi thêm đơn hàng cho khách", ex);
            }
        }

        public async Task<OrderDTO> UpdateOrderStatusAsync(int id, OrderStatusModel orderStatus)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng");

                if (orderStatus.OrderStatus == "Cancelled")
                {
                    var orderDetails = await _context.OrderDetails.Where(od => od.OrderID == id).ToListAsync();
                    foreach (var orderDetail in orderDetails)
                    {
                        var book = await _context.Books.FindAsync(orderDetail.BookID);  
                        if (book != null)
                        {
                            book.StockQuantity += orderDetail.Quantity; 
                        }
                    }
                }
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
    }
}
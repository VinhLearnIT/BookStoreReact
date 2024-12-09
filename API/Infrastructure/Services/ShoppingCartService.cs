using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ShoppingCartService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShoppingCartDTO>> GetCartByCustomerIdAsync(int id)
        {
            try
            {
                var carts = await _context.ShoppingCarts
                                           .Where(sc => sc.CustomerID == id)
                                           .Include(sc => sc.Book)
                                           .ToListAsync();

                return _mapper.Map<IEnumerable<ShoppingCartDTO>>(carts);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy giỏ hàng của khách hàng" + ex.Message, ex);
            }
        }

        public async Task<object> GetCountCartByCustomerIdAsync(int id)
        {
            try
            {
                var count= await _context.ShoppingCarts.CountAsync(c => c.CustomerID == id);                                         
                return new { cartCount = count };
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy giỏ hàng của khách hàng" + ex.Message, ex);
            }
        }

        public async Task<object> CheckStockQuantity(int bookID, int quantity)
        {
            try
            {
                var book = await _context.Books.FindAsync(bookID)
                                    ?? throw new NotFoundException("Không tìm thấy sách");
                if (quantity > book.StockQuantity)
                {
                    throw new BadRequestException($"Số lượng yêu cầu ({quantity}) vượt quá số lượng hiện có ({book.StockQuantity}).");
                }
                return new { message = "Đủ số lượng" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<object> AddToCartAsync(ShoppingCartDTO shoppingCartDto)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerID == shoppingCartDto.CustomerID && c.IsDeleted == false)
                    ?? throw new UnauthorizedException("Không tìm thấy khách hàng");

                var book = await _context.Books.FindAsync(shoppingCartDto.BookID)
                                    ?? throw new NotFoundException("Không tìm thấy sách");

                var existingCart = await _context.ShoppingCarts
                    .FirstOrDefaultAsync(sc => sc.CustomerID == shoppingCartDto.CustomerID &&
                                                sc.BookID == shoppingCartDto.BookID);
                

                if (existingCart != null)
                {
                    var sumQuantity = existingCart.Quantity + shoppingCartDto.Quantity;

                    if (sumQuantity > book.StockQuantity)
                    {
                        throw new BadRequestException($"Số lượng trong giỏ hàng ({sumQuantity}) vượt quá số lượng hiện có ({book.StockQuantity}).");
                    }

                    existingCart.Quantity += shoppingCartDto.Quantity;
                    await _context.SaveChangesAsync();
                    return new { message = "Old" };
                }

                if (shoppingCartDto.Quantity > book.StockQuantity)
                {
                    throw new BadRequestException($"Số lượng yêu cầu ({shoppingCartDto.Quantity}) vượt quá số lượng hiện có ({book.StockQuantity}).");
                }

                var cart = _mapper.Map<ShoppingCart>(shoppingCartDto);
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
                return new { message = "New" };

            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng" + ex.Message, ex);
            }
        }

        public async Task<ShoppingCartDTO> UpdateCartAsync(int id, ShoppingCartDTO shoppingCartDto)
        {
            try
            {
                if (shoppingCartDto == null || shoppingCartDto.Quantity <= 0)
                {
                    throw new BadRequestException("Dữ liệu giỏ hàng không hợp lệ.");
                }

                var cart = await _context.ShoppingCarts.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy giỏ hàng");

                var book = await _context.Books.FindAsync(shoppingCartDto.BookID)
                    ?? throw new NotFoundException("Không tìm thấy sách");

                cart.Quantity = shoppingCartDto.Quantity;

                await _context.SaveChangesAsync();

                return _mapper.Map<ShoppingCartDTO>(cart);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật giỏ hàng" + ex.Message, ex);
            }
        }

        public async Task<object> DeleteCartAsync(int id)
        {
            try
            {
                var cart = await _context.ShoppingCarts.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy giỏ hàng");

                _context.ShoppingCarts.Remove(cart);
                await _context.SaveChangesAsync();

                return new { message = "Xóa thành công!" };

            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa sản phẩm khỏi giỏ hàng" + ex.Message, ex);
            }
        }
    }
}

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

        // Lấy giỏ hàng của một khách hàng cụ thể (dựa trên CustomerID)
        public async Task<IEnumerable<ShoppingCartDTO>> GetCartByCustomerIdAsync(int customerId)
        {
            try
            {
                var carts = await _context.ShoppingCarts
                                           .Where(sc => sc.CustomerID == customerId)
                                           .Include(sc => sc.Book)
                                           .ToListAsync();

                return _mapper.Map<IEnumerable<ShoppingCartDTO>>(carts);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy giỏ hàng của khách hàng" + ex.Message, ex);
            }
        }

        public async Task<ShoppingCartDTO> AddToCartAsync(ShoppingCartDTO shoppingCartDto)
        {
            try
            {
                var book = await _context.Books.FindAsync(shoppingCartDto.BookID)
                    ?? throw new NotFoundException("Không tìm thấy sách");

                var existingCart = await _context.ShoppingCarts
                                                 .FirstOrDefaultAsync(sc => sc.CustomerID == shoppingCartDto.CustomerID &&
                                                                            sc.BookID == shoppingCartDto.BookID);

                if (existingCart != null)
                {
                    existingCart.Quantity += shoppingCartDto.Quantity;  // Update quantity if already in the cart
                    await _context.SaveChangesAsync();
                    return _mapper.Map<ShoppingCartDTO>(existingCart);
                }

                var cart = _mapper.Map<ShoppingCart>(shoppingCartDto);
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();

                shoppingCartDto.CartID = cart.CartID;
                return _mapper.Map<ShoppingCartDTO>(cart);
            }
            catch (NotFoundException)
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
                var cart = await _context.ShoppingCarts.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy giỏ hàng");

                var book = await _context.Books.FindAsync(shoppingCartDto.BookID)
                    ?? throw new NotFoundException("Không tìm thấy sách");

                cart.Quantity = shoppingCartDto.Quantity;
                cart.BookID = shoppingCartDto.BookID;
                cart.Book = book;

                await _context.SaveChangesAsync();

                return _mapper.Map<ShoppingCartDTO>(cart);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật giỏ hàng" + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteCartAsync(int id)
        {
            try
            {
                var cart = await _context.ShoppingCarts.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy giỏ hàng");

                _context.ShoppingCarts.Remove(cart);
                await _context.SaveChangesAsync();

                return true;
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

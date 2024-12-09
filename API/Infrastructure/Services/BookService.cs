using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            try
            {
                var books = await _context.Books.ToListAsync();
                return _mapper.Map<IEnumerable<BookDTO>>(books);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy tất cả sách", ex);
            }
        }

        public async Task<BookDTO> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == id);
                return book == null ? throw new NotFoundException("Không tìm thấy sách") : _mapper.Map<BookDTO>(book);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin sách", ex);
            }
        }
        public async Task<IEnumerable<BookDTO>> GetNewInfoBooksAsync(string arrBookID)
        {
            try
            {              
                var bookIDs = arrBookID.Split(", ");
                var newInfoBooks = await _context.Books
                    .Where(b => bookIDs.Contains(b.BookID.ToString()))
                    .ToListAsync();

                return _mapper.Map<IEnumerable<BookDTO>>(newInfoBooks);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi khi lấy sách liên quan", ex);
            }
        }

        public async Task<object> GetTopBooksAsync()
        {
            try
            {
                var newestBooks = await _context.Books
                    .OrderByDescending(b => b.PublishedDate)
                    .Take(4)
                    .ToListAsync();

                var bestSellingBooks = await _context.OrderDetails
                    .Where(od => od.Order.OrderStatus != "Canceled")
                    .GroupBy(od => od.BookID)
                    .Select(g => new
                    {
                        BookID = g.Key,
                        BookName = g.First().Book.BookName,
                        Author = g.First().Book.Author,
                        Publisher = g.First().Book.Publisher,
                        PublishedDate = g.First().Book.PublishedDate,
                        Price = g.First().Book.Price,
                        StockQuantity = g.First().Book.StockQuantity,
                        Description = g.First().Book.Description,
                        ImagePath = g.First().Book.ImagePath,
                        Categories = g.First().Book.Categories,
                        TotalQuantitySold = g.Sum(od => od.Quantity)
                    })
                    .OrderByDescending(b => b.TotalQuantitySold)
                    .Take(4)
                    .ToListAsync();       
                return new
                {
                    TopNew = newestBooks,
                    TopBestSell = bestSellingBooks
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách top sách", ex);
            }
        }
        public async Task<IEnumerable<BookDTO>> SearchBooksAsync(string searchName)
        {
            try
            {
                var resultBooks = await _context.Books
                                         .Where(b => b.BookName.Contains(searchName))
                                         .AsNoTracking()
                                         .ToListAsync();
                return _mapper.Map<IEnumerable<BookDTO>>(resultBooks);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi khi lấy sách liên quan", ex);
            }
        }


        public async Task<IEnumerable<BookDTO>> GetRelatedBooksAsync(int bookID)
        {
            try
            {
                var currentBook = await _context.Books
                    .Where(b => b.BookID == bookID)
                    .FirstOrDefaultAsync();

                if (currentBook == null)
                {
                    throw new NotFoundException("Sách không tồn tại");
                }

                var categories = currentBook.Categories.Split(", ");

                var relatedBooks = await _context.Books
                    .Where(b => b.BookID != bookID) 
                    .ToListAsync(); 

                var relatedBooksFiltered = relatedBooks
                    .Where(b => b.Categories.Split(", ").Intersect(categories).Any())
                    .OrderByDescending(b => b.PublishedDate)
                    .Take(4)
                    .ToList();

                return _mapper.Map<IEnumerable<BookDTO>>(relatedBooks);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi khi lấy sách liên quan", ex);
            }
        }


        public async Task<BookDTO> CreateBookAsync(BookDTO bookDto)
        {
            try
            {
                Validate(bookDto);

                var book = _mapper.Map<Book>(bookDto);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                bookDto.BookID = book.BookID;

                return _mapper.Map<BookDTO>(book);
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
                throw new Exception("Có lỗi xảy ra khi tạo sách mới" + ex.Message, ex);
            }
        }

        public async Task<BookDTO> UpdateBookAsync(int id, BookDTO bookDto)
        {
            try
            {
                var book = await _context.Books.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy sách");

                Validate(bookDto);

                book.BookName = bookDto.BookName;
                book.Author = bookDto.Author;
                book.Publisher = bookDto.Publisher;
                book.PublishedDate = bookDto.PublishedDate;
                book.Price = bookDto.Price;
                book.StockQuantity = bookDto.StockQuantity;
                book.Description = bookDto.Description;
                book.ImagePath = bookDto.ImagePath;
                book.Categories = bookDto.Categories;

                await _context.SaveChangesAsync();

                return _mapper.Map<BookDTO>(book);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật sách" + ex.Message, ex);
            }
        }

        public async Task<object> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy sách");

                var isUsedInCart = await _context.ShoppingCarts
            .           AnyAsync(cart => cart.BookID == id);

                var isUsedInOrders = await _context.OrderDetails
                    .AnyAsync(od => od.BookID == id);

                if (isUsedInCart || isUsedInOrders)
                {
                    throw new BadRequestException("Không thể xóa sách vì nó đang được sử dụng trong giỏ hàng hoặc đơn hàng."); ;
                }
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return new { message = "Xóa sách thành công!" };
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
                throw new Exception("Có lỗi xảy ra khi xóa sách " + ex.Message, ex);
            }
        }


        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            try
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                var imageName = imageFile.FileName.Replace(" ", "");
                var path = Path.Combine(uploadFolder, imageName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return imageName;
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi xảy ra khi thêm hình ảnh " + ex.Message, ex);
            }

        }

        private static void Validate(BookDTO bookDto)
        {

            if (bookDto.Price < 0)
            {
                throw new BadRequestException("Giá sách phải lớn hơn 0");
            }

            if (bookDto.StockQuantity < 0)
            {
                throw new BadRequestException("Số lượng sách phải lớn hơn 0");
            }
        }
    }
}

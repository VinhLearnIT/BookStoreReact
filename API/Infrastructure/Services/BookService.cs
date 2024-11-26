using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy sách");

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NotFoundException)
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
                var imageAPI = @"https://localhost:7138/api/images/" + imageName;
                var path = Path.Combine(uploadFolder, imageName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return imageAPI;
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

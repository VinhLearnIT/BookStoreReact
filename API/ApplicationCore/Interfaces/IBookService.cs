using ApplicationCore.DTOs;
using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO> GetBookByIdAsync(int id);
        Task<BookDTO> CreateBookAsync(BookDTO bookDto);
        Task<BookDTO> UpdateBookAsync(int id, BookDTO bookDto);
        Task<bool> DeleteBookAsync(int id);
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}

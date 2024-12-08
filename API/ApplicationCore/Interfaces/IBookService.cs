using ApplicationCore.DTOs;
using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO> GetBookByIdAsync(int id);
        Task<IEnumerable<BookDTO>> SearchBooksAsync(string searchName);
        Task<object> GetTopBooksAsync();
        Task<IEnumerable<BookDTO>> GetRelatedBooksAsync(int bookID);
        Task<IEnumerable<BookDTO>> GetNewInfoBooksAsync(string arrBookID);
        Task<BookDTO> CreateBookAsync(BookDTO bookDto);
        Task<BookDTO> UpdateBookAsync(int id, BookDTO bookDto);
        Task<object> DeleteBookAsync(int id);
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}

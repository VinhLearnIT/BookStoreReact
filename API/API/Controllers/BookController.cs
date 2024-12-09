using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            return Ok(await _bookService.GetAllBooksAsync());
        }

        [HttpGet("{searchName}")]
        public async Task<IActionResult> SearchBooks(string searchName)
        {
            Console.WriteLine($"Searching for: {searchName}");
            return Ok(await _bookService.SearchBooksAsync(searchName));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            return Ok(await _bookService.GetBookByIdAsync(id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRelatedBooks(int id)
        {
            return Ok(await _bookService.GetRelatedBooksAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetTopBooks()
        {
            return Ok(await _bookService.GetTopBooksAsync());
        }

        [HttpGet("{arrBookID}")]
        public async Task<IActionResult> GetNewInfoBooks(string arrBookID)
        {
            return Ok(await _bookService.GetNewInfoBooksAsync(arrBookID));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateBook([FromBody] BookDTO bookDto)
        {
            return Ok(await _bookService.CreateBookAsync(bookDto));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            return Ok(new { ImagePath = await _bookService.UploadImageAsync(imageFile) } );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDTO bookDto)
        {
            return Ok(await _bookService.UpdateBookAsync(id, bookDto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            return Ok(await _bookService.DeleteBookAsync(id));
        }
    }
}

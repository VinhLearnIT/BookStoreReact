using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            return Ok(await _bookService.GetBookByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDTO bookDto)
        {
            return Ok(await _bookService.CreateBookAsync(bookDto));
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            return Ok(new { ImagePath = await _bookService.UploadImageAsync(imageFile) } );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDTO bookDto)
        {
            return Ok(await _bookService.UpdateBookAsync(id, bookDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            return Ok(await _bookService.DeleteBookAsync(id));
        }
    }
}

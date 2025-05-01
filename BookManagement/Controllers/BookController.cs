using BookManagement.Models;
using BookManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        private readonly ILogger<BookController> _logger;
        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }


        [HttpGet(Name = "GetBooks")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync().ConfigureAwait(false);
            if (books is not null)
            {
                return Ok(books);
            }
            else
            {
                return NotFound("No books found");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookById(id).ConfigureAwait(false);
            if (book is not null)
            {
                return Ok(book);
            }
            else
            {
                return NotFound("Book not found");
            }
        }
        [HttpPost]
        public async Task<ActionResult<BookDTO>> Create([FromBody] CreateBookDTO dto)
        {
            var created = await _bookService.AddBookAsync(dto);
            return CreatedAtAction(nameof(Create), new { id = created.id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateBookDTO dto)
        {
            await _bookService.UpdateBookAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}

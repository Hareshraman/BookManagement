using BookManagement.Models;

namespace BookManagement.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();  
        Task<BookDTO>? GetBookById(Guid id); 
        Task<BookDTO> AddBookAsync(CreateBookDTO bookDTO);

        Task<BookDTO> UpdateBookAsync(Guid id, CreateBookDTO bookDTO); 

        Task<bool> DeleteBookAsync(Guid id);
    }
}

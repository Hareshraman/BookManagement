using BookManagement.Models;

namespace BookManagement.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();    
        Task<Book> GetBookById(Guid id);    
        Task AddBookAsync(Book book); 

        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(Book book);

        Task SaveChangesAsync();

    }
}

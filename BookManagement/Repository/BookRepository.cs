using BookManagement.Database;
using BookManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _context;
        public BookRepository(BookContext context)
        {
            _context = context;
        }
        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);    
        }

        public async Task FindBookByIdAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }
        }   

        public async Task DeleteBookAsync(Book book)
        {
            _context.Books.Remove(book);

        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.AsNoTracking().ToListAsync();  
        }

        public async Task<Book?> GetBookById(Guid id)
        {
           return await _context.Books.FindAsync(id);
        }

        public Task SaveChangesAsync()
        {
             return _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            var existingItem = await _context.Books.FindAsync(book.id);
            if (existingItem != null)
            {
                existingItem.Title = book.Title;
                existingItem.Author = book.Author;
                existingItem.PublishedDate = book.PublishedDate;
                existingItem.isAvailable = book.isAvailable;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Book not found");
            }
        }
    }
}

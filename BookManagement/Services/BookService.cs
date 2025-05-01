using BookManagement.Models;
using BookManagement.Repository;
using System.Collections.Generic;

namespace BookManagement.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<BookDTO> AddBookAsync(CreateBookDTO bookDTO)
        {
            if (bookDTO != null)
            {
                var book = new Book(bookDTO.Title, bookDTO.Author, bookDTO.PublishedDate, bookDTO.isAvailable);
                await _bookRepository.AddBookAsync(book);
                await _bookRepository.SaveChangesAsync();

                return new BookDTO
                {
                    id = book.id,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedDate = book.PublishedDate,
                    isAvailable = book.isAvailable
                };
            }
            else
            {
                throw new ArgumentException("Invalid book details");

            }
        }
        public async Task<bool> DeleteBookAsync(Guid id)
        {
            Book book = await _bookRepository.GetBookById(id);
            if (book is not null)
            {
                await _bookRepository.DeleteBookAsync(book);
                return true;
            }
            return false;
        }


        public Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            IEnumerable<Book> books = _bookRepository.GetAllBooksAsync().Result.ToList();
            if (books is not null)
            {
                return Task.FromResult(books.Select(b => new BookDTO
                {
                    id = b.id,
                    Title = b.Title,
                    Author = b.Author,
                    PublishedDate = b.PublishedDate,
                    isAvailable = b.isAvailable
                }));

            }
            return null;
        }

        public async Task<BookDTO>? GetBookById(Guid id)
        {
            Book book = await _bookRepository.GetBookById(id);
            if (book != null)
            {
                return new BookDTO
                {
                    id = book.id,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedDate = book.PublishedDate,
                    isAvailable = book.isAvailable
                };
            }
            return null;
        }

        public async Task<BookDTO> UpdateBookAsync(Guid id, CreateBookDTO bookDTO)
        {
            if (bookDTO is not null)
            {
                var book = await _bookRepository.GetBookById(id);
                if (book is not null)
                {
                    book.Title = bookDTO.Title;
                    book.Author = bookDTO.Author;
                    book.PublishedDate = bookDTO.PublishedDate;
                    book.isAvailable = bookDTO.isAvailable;
                    await _bookRepository.UpdateBookAsync(book);
                    await _bookRepository.SaveChangesAsync();
                    return new BookDTO
                    {
                        id = book.id,
                        Title = book.Title,
                        Author = book.Author,
                        PublishedDate = book.PublishedDate,
                        isAvailable = book.isAvailable
                    };
                }
                else
                {

                    throw new ArgumentException("Book not found");
                }

            }
            else
            {
                throw new ArgumentException("Invalid book details");
            }
        }
    }
}

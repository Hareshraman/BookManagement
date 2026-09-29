using BookManagement.Database;
using BookManagement.Models;
using BookManagement.Repository;
using BookManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Tests;

public class BookServiceTests
{
    private static BookContext NewContext() =>
        new(new DbContextOptionsBuilder<BookContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static CreateBookDTO SampleDto(string title = "Dune", string author = "Frank Herbert") =>
        new()
        {
            Title = title,
            Author = author,
            PublishedDate = new DateTime(1965, 8, 1),
            isAvailable = true
        };

    [Fact]
    public async Task AddBookAsync_PersistsBookAndReturnsDto()
    {
        using var context = NewContext();
        var service = new BookService(new BookRepository(context));

        var created = await service.AddBookAsync(SampleDto());

        Assert.NotEqual(Guid.Empty, created.id);
        Assert.Equal("Dune", created.Title);
        Assert.Single(await context.Books.ToListAsync());
    }

    [Fact]
    public async Task GetAllBooksAsync_ReturnsEveryStoredBook()
    {
        using var context = NewContext();
        var service = new BookService(new BookRepository(context));
        await service.AddBookAsync(SampleDto());
        await service.AddBookAsync(SampleDto("Neuromancer", "William Gibson"));

        var books = await service.GetAllBooksAsync();

        Assert.Equal(
            new[] { "Dune", "Neuromancer" },
            books.Select(b => b.Title).OrderBy(t => t).ToArray());
    }

    [Fact]
    public async Task GetBookById_ReturnsNullForUnknownId()
    {
        using var context = NewContext();
        var service = new BookService(new BookRepository(context));

        Assert.Null(await service.GetBookById(Guid.NewGuid())!);
    }

    [Fact]
    public async Task UpdateBookAsync_OverwritesStoredFields()
    {
        using var context = NewContext();
        var service = new BookService(new BookRepository(context));
        var created = await service.AddBookAsync(SampleDto());

        var updated = await service.UpdateBookAsync(
            created.id,
            new CreateBookDTO
            {
                Title = "Dune Messiah",
                Author = "Frank Herbert",
                PublishedDate = new DateTime(1969, 10, 15),
                isAvailable = false
            });

        Assert.Equal("Dune Messiah", updated.Title);
        Assert.False(updated.isAvailable);
        var stored = await context.Books.SingleAsync();
        Assert.Equal("Dune Messiah", stored.Title);
    }

    [Fact]
    public async Task UpdateBookAsync_ThrowsForUnknownId()
    {
        using var context = NewContext();
        var service = new BookService(new BookRepository(context));

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.UpdateBookAsync(Guid.NewGuid(), SampleDto()));
    }

    [Fact]
    public async Task DeleteBookAsync_RemovesBookFromTheDatabase()
    {
        using var context = NewContext();
        var service = new BookService(new BookRepository(context));
        var created = await service.AddBookAsync(SampleDto());

        Assert.True(await service.DeleteBookAsync(created.id));

        Assert.Empty(await context.Books.ToListAsync());
    }

    [Fact]
    public async Task DeleteBookAsync_ReturnsFalseForUnknownId()
    {
        using var context = NewContext();
        var service = new BookService(new BookRepository(context));

        Assert.False(await service.DeleteBookAsync(Guid.NewGuid()));
    }
}

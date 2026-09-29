using BookManagement.Models;

namespace BookManagement.Tests;

public class BookTests
{
    [Fact]
    public void Constructor_AssignsIdAndFields()
    {
        var published = new DateTime(1965, 8, 1);

        var book = new Book("Dune", "Frank Herbert", published, true);

        Assert.NotEqual(Guid.Empty, book.id);
        Assert.Equal("Dune", book.Title);
        Assert.Equal("Frank Herbert", book.Author);
        Assert.Equal(published, book.PublishedDate);
        Assert.True(book.isAvailable);
    }

    [Theory]
    [InlineData("", "Frank Herbert")]
    [InlineData("Dune", "")]
    public void Constructor_RejectsMissingTitleOrAuthor(string title, string author)
    {
        Assert.Throws<ArgumentException>(
            () => new Book(title, author, new DateTime(1965, 8, 1), true));
    }

    [Fact]
    public void Constructor_RejectsFuturePublishedDate()
    {
        Assert.Throws<ArgumentException>(
            () => new Book("Dune", "Frank Herbert", DateTime.Now.AddDays(1), true));
    }
}

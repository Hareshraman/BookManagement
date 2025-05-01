using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Models
{
    public class Book
    {
        public Guid id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }

        public bool isAvailable { get; set; } = true;


        public Book(string title, string author, DateTime publishedDate,bool isAvailable)
        {
            if (ValidateDomain(title, author, publishedDate))
            {
                Title = title;
                Author = author;
                PublishedDate = publishedDate;
                id = Guid.NewGuid();
                this.isAvailable = isAvailable;
            }
            else
            {
                throw new ArgumentException("Invalid book details");
            }
        }


        public bool ValidateDomain(string title, string author, DateTime publishedDate)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title cannot be null or empty");
            }
            if (string.IsNullOrEmpty(author))
            {
                throw new ArgumentException("Author cannot be null or empty");

            }
            if (publishedDate > DateTime.Now)
            {
                throw new ArgumentException("Published date cannot be in the future");
            }
            return true;
        }

    }

    //DTO's

    public class BookDTO
    {
        public Guid id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool isAvailable { get; set; } = true;
    }

    public class CreateBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }

        public bool isAvailable { get; set; } = true;
    }
}
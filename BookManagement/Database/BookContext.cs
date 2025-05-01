using BookManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Database
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("BookManagement");        

            modelBuilder.Entity<Book>().Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Book>().Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100); 
            modelBuilder.Entity<Book>().Property(b => b.PublishedDate)
                .IsRequired();
            modelBuilder.Entity<Book>().Property(b => b.isAvailable)
                .IsRequired();  

        }
    }
}

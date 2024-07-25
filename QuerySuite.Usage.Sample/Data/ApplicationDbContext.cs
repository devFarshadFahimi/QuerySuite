using Microsoft.EntityFrameworkCore;

namespace QuerySuite.Usage.Sample.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(p =>
        {
            p.HasKey(q => q.Id);
            p.Property(q => q.FirstName).HasMaxLength(50);
            p.Property(q => q.LastName).HasMaxLength(50);
            p.HasData([
                new Author()
                {
                    Id = 1,
                    FirstName = "Farshad",
                    LastName = "Fahimi"
                },
                new Author()
                {
                    Id = 2,
                    FirstName = "Omid",
                    LastName = "Rezaei"
                },new Author()
                {
                    Id = 3,
                    FirstName = "Nabi",
                    LastName = "Karampour"
                }
            ]);
        });

        modelBuilder.Entity<Book>(p =>
        {
            p.HasKey(q => q.Id);
            p.Property(q => q.Title).HasMaxLength(150);
            p.HasOne(q => q.Author)
                .WithMany(q => q.Books)
                .HasForeignKey(q => q.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            p.HasData([
                new Book() { Id = 1, AuthorId = 1, Title = "Book-Farshad-1" },
                new Book() { Id = 2, AuthorId = 1, Title = "Book-Farshad-2" },
                new Book() { Id = 3, AuthorId = 1, Title = "Book-Farshad-3" },
                new Book() { Id = 4, AuthorId = 3, Title = "Book-Nabi-4" },
                new Book() { Id = 5, AuthorId = 3, Title = "Book-Nabi-5" },
                new Book() { Id = 6, AuthorId = 3, Title = "Book-Nabi-6" },
                new Book() { Id = 7, AuthorId = 3, Title = "Book-Nabi-7" },
                new Book() { Id = 8, AuthorId = 3, Title = "Book-Nabi-8" },
                    
                new Book() { Id = 9, AuthorId =  2, Title = "Book-Omid-9" },
                new Book() { Id = 10, AuthorId = 2, Title = "Book-Omid-10" },
                new Book() { Id = 11, AuthorId = 2, Title = "Book-Omid-11" },
                new Book() { Id = 12, AuthorId = 2, Title = "Book-Omid-12" }, new Book() { Id = 13, AuthorId = 2, Title = "Book-Omid-13" },
            ]);
        });
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
}

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public ICollection<Book> Books { get; set; } = [];
}
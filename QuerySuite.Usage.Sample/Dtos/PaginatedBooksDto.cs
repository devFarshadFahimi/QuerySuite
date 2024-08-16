using QuerySuite.Core.Attributes;

namespace QuerySuite.Usage.Sample.Dtos;

public class PaginatedBooksDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public  bool IsPublished { get; set; }

    [MapToEntity("Author.Id")]
    public int AuthorId { get; init; }

    [MapToEntity("Author.FirstName")]
    public string AuthorFirstName { get; set; } = null!;

    [MapToEntity("Author.LastName")]
    public string AuthorLastName { get; set; } = null!;
}
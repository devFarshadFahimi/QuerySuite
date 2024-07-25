using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuerySuite.Contracts.Extensions;
using QuerySuite.Contracts.Models;
using QuerySuite.Core.Attributes;
using QuerySuite.Usage.Sample.Data;

namespace QuerySuite.Usage.Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetPaginatedBooks([FromBody] QuerySuiteParams @params)
    {
        var result = await dbContext.Books
            .Include(p => p.Author)
            .ToPaginatedDataAsync<Book, PaginatedBooksDto>(@params);
        
            return Ok(result);
    }
}

public class PaginatedBooksDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    [MapToEntity("Author.Id")] public int AuthorId { get; init; }

    [MapToEntity("Author.FirstName")] public string AuthorFirstName { get; set; } = null!;

    [MapToEntity("Author.LastName")] public string AuthorLastName { get; set; } = null!;
}
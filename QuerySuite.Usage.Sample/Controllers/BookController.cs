using Microsoft.AspNetCore.Mvc;
using QuerySuite.Contracts.Extensions;
using QuerySuite.Contracts.Models;
using QuerySuite.Usage.Sample.Data;
using QuerySuite.Usage.Sample.Dtos;
using QuerySuite.Usage.Sample.Specs;

namespace QuerySuite.Usage.Sample.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BookController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetPaginatedBooks([FromBody] QuerySuiteParams @params)
    {
        var result = await dbContext.Books
            .ToPaginatedDataAsync<Book, PaginatedBooksDto>(@params);
            return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetSpecMixedWithPaginatedBooks([FromBody] QuerySuiteParams @params, bool isPublished)
    {
        var result = await dbContext.Books
            .Where(new ByBookIsPublished(isPublished))
            .ToPaginatedDataAsync<Book, PaginatedBooksDto>(@params);
            return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByBookTitleAndIsPublished(string bookTitle, bool isPublished, CancellationToken cancellationToken)
    {
        var spec = new ByBookTitle(bookTitle).And(new ByBookIsPublished(isPublished));
        var result = await dbContext.Books
            .ToSpecListAsync(spec, cancellationToken);
            return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByAuthorName(string authorFirstname)
    {
        var result = await dbContext.Books
            .ToSpecListAsync(new ByAuthorFirstName(authorFirstname));
            return Ok(result);
    }
}
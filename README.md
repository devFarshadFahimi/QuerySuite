### README.md for Paginizer

---

# Paginizer

**Paginizer** is a robust and flexible .NET library designed to provide easy-to-use pagination, filtering, and sorting functionalities for Entity Framework Core queries. It abstracts EF Core specifics, making it compatible with different versions of EF Core and allowing you to manage complex data retrieval scenarios effortlessly.

## Features

- **Pagination**: Easily paginate your IQueryable queries.
- **Filtering**: Apply dynamic filters based on various conditions.
- **Sorting**: Sort your data by multiple columns.
- **Compatibility**: Compatible with different versions of EF Core.

## Installation

You can install Paginizer via NuGet Package Manager:

```sh
dotnet add package Paginizer
```

Or via the NuGet Package Manager Console in Visual Studio:

```sh
Install-Package Paginizer
```

## Getting Started

### Step 1: Define Pagination Parameters

Define the pagination parameters, filter parameters, and sorting parameters.

```csharp
public class PaginationParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; }
    public bool SortDescending { get; set; }
    public List<FilterParams> Filters { get; set; } = new List<FilterParams>();
}

public class FilterParams
{
    public string FilterColumn { get; set; }
    public string FilterQuery { get; set; }
    public string FilterCondition { get; set; } // equals, contains, greater, lower, etc.
}
```

### Step 2: Define Paginated Result

Define a class to hold the paginated results.

```csharp
public class PaginatedResult<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
```

### Step 3: Apply Pagination, Filtering, and Sorting

Use the provided extension methods to apply pagination, filtering, and sorting.

```csharp
using Paginizer;

public static class IQueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> query, PaginationParams paginationParams, IAsyncQueryProvider asyncQueryProvider)
    {
        // Apply filters
        foreach (var filter in paginationParams.Filters)
        {
            query = ApplyFilter(query, filter);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(paginationParams.SortColumn))
        {
            query = ApplySorting(query, paginationParams.SortColumn, paginationParams.SortDescending);
        }

        // Get total count
        int totalCount = await asyncQueryProvider.CountAsync(query);

        // Apply pagination
        var items = await asyncQueryProvider.ToListAsync(
            query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                 .Take(paginationParams.PageSize)
        );

        return new PaginatedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        };
    }

    private static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, FilterParams filter)
    {
        // Logic for applying filters
        return query;
    }

    private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortColumn, bool sortDescending)
    {
        // Logic for applying sorting
        return query;
    }
}
```

### Step 4: Setup Dependency Injection

In your application, set up dependency injection to use the `IAsyncQueryProvider` for EF Core.

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAsyncQueryProvider, EfCoreAsyncQueryProvider>();

        // Other service registrations
    }

    // Other configurations
}
```

### Step 5: Use in Controller

Use the extension method in your controller to paginate, filter, and sort your data.

```csharp
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAsyncQueryProvider _asyncQueryProvider;

    public UsersController(ApplicationDbContext context, IAsyncQueryProvider asyncQueryProvider)
    {
        _context = context;
        _asyncQueryProvider = asyncQueryProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationParams paginationParams)
    {
        var query = _context.Users.Include(u => u.Role).AsQueryable();

        // Apply pagination
        var result = await query.ToPaginatedResultAsync(paginationParams, _asyncQueryProvider);
        return Ok(result);
    }
}
```

## Example Project

An example project is included in the repository to demonstrate how to use Paginizer. The example project includes a basic setup with an ASP.NET Core Web API and Entity Framework Core.

### Example Project Structure

```
Paginizer
│
├── src
│   ├── Paginizer
│   │   ├── IQueryableExtensions.cs
│   │   ├── IAsyncQueryProvider.cs
│   │   └── EfCoreAsyncQueryProvider.cs
│   └── Paginizer.Sample
│       ├── Controllers
│       │   └── UsersController.cs
│       ├── Data
│       │   ├── ApplicationDbContext.cs
│       │   └── Models
│       │       ├── User.cs
│       │       └── Role.cs
│       ├── Program.cs
│       ├── Startup.cs
│       └── Paginizer.Sample.csproj
├── tests
│   └── Paginizer.Tests
│       ├── PaginizerTests.cs
│       └── Paginizer.Tests.csproj
├── Paginizer.sln
└── README.md
```

## Contributing

We welcome contributions to Paginizer! If you have any ideas, suggestions, or issues, please open an issue or submit a pull request.

## License

Paginizer is licensed under the MIT License. See the LICENSE file for more details.

---

With this README file, users will have a clear understanding of how to install, configure, and use the Paginizer library in their .NET applications. The example project provides a concrete demonstration, making it easier for users to get started quickly.
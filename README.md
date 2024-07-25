### README.md for QuerySuite

---

# QuerySuite

**QuerySuite** is a .NET library designed to simplify and enhance querying capabilities by providing robust pagination, filtering, and sorting functionalities for `IQueryable` collections. This library aims to improve the efficiency and readability of your data access layer in ASP.NET Core applications.

## Features

- **Pagination:** Easy-to-use methods for paginating query results.
- **Filtering:** Dynamic filters with various conditions.
- **Sorting:** Flexible sorting on multiple columns.
- **Nested Property Handling:** Map DTO properties to nested entity properties.

## Installation

Install QuerySuite via NuGet Package Manager:

```sh
dotnet add package QuerySuite
```

Or via the NuGet Package Manager Console in Visual Studio:

```sh
Install-Package QuerySuite
```

## Getting Started

### Step 1: Define Pagination Parameters

Define the pagination, filter, and sorting parameters.

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

Create a class to hold the paginated results.

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
using QuerySuite;

public static class IQueryableExtensions
{
    public static async Task<PaginatedResult<TModel>> ToPaginatedResultAsync<TEntity, TModel>(this IQueryable<TEntity> query, PaginationParams paginationParams)
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
        int totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(GetSelectExpression<TEntity, TModel>())
            .ToListAsync();

        return new PaginatedResult<TModel>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        };
    }

    private static IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, FilterParams filter)
    {
        // Logic for applying filters
        return query;
    }

    private static IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> query, string sortColumn, bool sortDescending)
    {
        // Logic for applying sorting
        return query;
    }

    private static Expression<Func<TEntity, TModel>> GetSelectExpression<TEntity, TModel>()
    {
        var entityParam = Expression.Parameter(typeof(TEntity), "e");
        var bindings = typeof(TModel).GetProperties()
            .Select(modelProp => Expression.Bind(modelProp, GetNestedPropertyExpression(entityParam, modelProp)))
            .ToList();
        return Expression.Lambda<Func<TEntity, TModel>>(Expression.MemberInit(Expression.New(typeof(TModel)), bindings), entityParam);
    }

    private static Expression GetNestedPropertyExpression(Expression parameter, PropertyInfo property)
    {
        var parts = property.GetCustomAttribute<MapToEntityAttribute>()?.EntityPropertyName.Split('.') ?? new[] { property.Name };
        Expression propertyAccess = parameter;
        foreach (var part in parts)
        {
            propertyAccess = Expression.Property(propertyAccess, part);
        }
        return propertyAccess;
    }
}
```

### Step 4: Setup Dependency Injection

Configure dependency injection in your application.

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Other service registrations
    }

    // Other configurations
}
```

### Step 5: Use in Controller

Use the extension method in your controller to handle data operations.

```csharp
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationParams paginationParams)
    {
        var query = _context.Users.Include(u => u.Role).AsQueryable();
        var result = await query.ToPaginatedResultAsync<User, UserDto>(paginationParams);
        return Ok(result);
    }
}
```

## Example Project

An example project is included in the repository to demonstrate how to use QuerySuite. The example includes a basic setup with an ASP.NET Core Web API and Entity Framework Core.

### Example Project Structure

```
QuerySuite
│
├── src
│   ├── QuerySuite
│   │   ├── IQueryableExtensions.cs
│   │   ├── IAsyncQueryProvider.cs
│   │   └── EfCoreAsyncQueryProvider.cs
│   └── QuerySuite.Sample
│       ├── Controllers
│       │   └── UsersController.cs
│       ├── Data
│       │   ├── ApplicationDbContext.cs
│       │   └── Models
│       │       ├── User.cs
│       │       └── Role.cs
│       ├── Program.cs
│       ├── Startup.cs
│       └── QuerySuite.Sample.csproj
├── tests
│   └── QuerySuite.Tests
│       ├── QuerySuiteTests.cs
│       └── QuerySuite.Tests.csproj
├── QuerySuite.sln
└── README.md
```

## Contributing

We welcome contributions! If you have any ideas, suggestions, or issues, please open an issue or submit a pull request.

## License

QuerySuite is licensed under the MIT License. See the LICENSE file for more details.

---

This README provides a comprehensive guide for users to understand and implement QuerySuite in their projects. Feel free to adjust the details to better match your project's specifics.
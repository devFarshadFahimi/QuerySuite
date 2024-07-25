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

In your controllers method, define QuerySuiteParams as input parameter to receive it from user.
* If SortColumn have a value, sorting will be applied to query. 
* Filters should be an array of FilterCriteria object.

```csharp
public class QuerySuiteParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortColumn { get; set; }
    public bool SortDescending { get; set; } = false;
    public List<FilterCriteria> Filters { get; set; } = [];
}

public class FilterCriteria
{
    public required string Column { get; set; }
    public required string Value { get; set; }
    // Equals = 1, Contains = 2, StartsWith = 3, EndsWith = 4, GreaterThan = 5, LessThan =6 , GreaterThanOrEqual = 7, LessThanOrEqual = 8
    public FilterCondition Condition { get; set; }
}
```

### Step 2: After calling "ToPaginatedDataAsync<YourEntity, YourDTO>(querySuiteParams)" on you Queryable, you will receive below output as result.

```csharp
public class QuerySuiteResult<TModel>(int totalRecords,
    int pageNumber,
    int pageSize,
    IEnumerable<TModel>? data = null)
{
    public static QuerySuiteResult<TModel> Create(int totalRecords,
        int pageNumber,
        int pageSize,
        IEnumerable<TModel>? data = null) 
            => new(totalRecords, pageNumber, pageSize, data);

    public IEnumerable<TModel> Data { get; set; } = data ?? [];

    public int TotalPages => (int)Math.Ceiling((decimal)TotalRecords / PageSize);
    public int TotalRecords { get; set; } = totalRecords;
    public int PageSize { get; set; } = pageSize;
    public int PageNumber { get; set; } = pageNumber;
}
```

In summary, if you want to know how to use it in action, check this directory <[Example Project](https://github.com/devFarshadFahimi/QuerySuite/tree/main/QuerySuite.Usage.Sample)> to see an example of QuerySuite usage.

## Contributing

We welcome contributions! If you have any ideas, suggestions, or issues, please open an issue or submit a pull request.

## License

QuerySuite is licensed under the MIT License. See the LICENSE file for more details.

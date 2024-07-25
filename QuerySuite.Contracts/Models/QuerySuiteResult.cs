namespace Paginizer.Contracts.Models;

public class QuerySuiteResult<TModel>(int totalRecords, int pageNumber , int pageSize, IEnumerable<TModel>? data = null)
{
    public static QuerySuiteResult<TModel> Create(int totalRecords, int pageNumber, int pageSize, IEnumerable<TModel>? data = null)
        => new(totalRecords, pageNumber, pageSize, data);

    public IEnumerable<TModel> Data { get; set; } = data ?? [];

    public int TotalPages => TotalRecords / PageSize;
    public int TotalRecords { get; set; } = totalRecords;
    public int PageSize { get; set; } = pageSize;
    public int PageNumber { get; set; } = pageNumber;
}

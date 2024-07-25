using QuerySuite.Core.Contracts;

namespace Paginizer.Contracts.Models;

public class QuerySuiteParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortColumn { get; set; }
    public bool SortDescending { get; set; } = false;
    public List<FilterCriteria> Filters { get; set; } = [];
}

using Microsoft.EntityFrameworkCore;
using QuerySuite.Contracts.Models;
using QuerySuite.Core;

namespace QuerySuite.Contracts.Extensions;

public static class QuerySuiteExtensions
{
    public async static Task<QuerySuiteResult<TModel>> ToPaginatedDataAsync<TEntity, TModel>(
        this IQueryable<TEntity> query,
        QuerySuiteParams querySuiteParams)
        where TModel : new()
    {
        // Apply filtering
        if (querySuiteParams.Filters.Any())
        {
            query = query.ApplyFilters<TEntity, TModel>(querySuiteParams.Filters);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(querySuiteParams.SortColumn))
        {
            query = query.ApplySort(querySuiteParams.SortColumn, querySuiteParams.SortDescending);
        }

        // Get total records count before pagination
        var totalRecords = await query.CountAsync();

        // Apply pagination
        var data = await query
            .Skip((querySuiteParams.PageNumber - 1) * querySuiteParams.PageSize)
            .Take(querySuiteParams.PageSize)
            .ProjectToType<TEntity, TModel>()
            .ToListAsync();

        // Return paginated result
        return QuerySuiteResult<TModel>.Create(totalRecords, querySuiteParams.PageNumber, querySuiteParams.PageSize, data);
    }
}
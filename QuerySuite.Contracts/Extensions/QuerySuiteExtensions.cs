using Microsoft.EntityFrameworkCore;
using QuerySuite.Contracts.Models;
using QuerySuite.Core;
using QuerySuite.Core.Contracts.SpecificationPattern;

namespace QuerySuite.Contracts.Extensions;

public static class QuerySuiteExtensions
{
    public static async Task<QuerySuiteResult<TModel>> ToPaginatedDataAsync<TEntity, TModel>(
        this IQueryable<TEntity> query,
        QuerySuiteParams querySuiteParams)
        where TModel : new() where TEntity : class
    {
        // Apply filtering
        if (querySuiteParams.Filters.Count != 0)
        {
            query = query.ApplyFilters<TEntity, TModel>(querySuiteParams.Filters);
        }

        // Apply sorting
        if (!string.IsNullOrEmpty(querySuiteParams.SortColumn))
        {
            query = query.ApplySort<TEntity, TModel>(querySuiteParams.SortColumn, querySuiteParams.SortDescending);
        }

        // Get total records count before pagination
        var totalRecords = await query.CountAsync();

        // Apply pagination
        query = query
            .Skip(querySuiteParams.TakeSize)
            .Take(querySuiteParams.PageSize);

        var data = await query
            .ProjectToType<TEntity, TModel>()
            .ToListAsync();
        // Return paginated result
        return QuerySuiteResult<TModel>.Create(totalRecords, querySuiteParams.PageNumber, querySuiteParams.PageSize,
            data);
    }
}
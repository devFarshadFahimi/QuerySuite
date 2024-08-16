using Microsoft.EntityFrameworkCore;
using QuerySuite.Core;
using QuerySuite.Core.Contracts.SpecificationPattern;

namespace QuerySuite.Contracts.Extensions;

public static class SpecificationExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> queryable, ISpecification<T> specification)
        where T : class
    {
        var query = BaseWhere(queryable, specification);
        return BaseOrderBy(query, specification);
    }

    private static IQueryable<T> BaseOrderBy<T>(IQueryable<T> queryable, ISpecification<T> specification) where T : class
    {
        var queryableResultWithIncludes = specification.OrderBy
            .Aggregate(queryable.AsNoTracking(), (current, orderBy) => current.OrderBy(orderBy));
        
        var secondaryResult = specification.OrderByDescending
            .Aggregate(queryableResultWithIncludes.AsNoTracking(),
                (current, orderByDesc) => current.OrderByDescending(orderByDesc));

        return secondaryResult;
    }

    public static async Task<T> ToSpecFirstAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.Where(spec).FirstAsync(cancellationToken);
    }


    public static async Task<T?> ToSpecFirstOrDefaultAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.Where(spec).FirstOrDefaultAsync(cancellationToken);
    }


    public static async Task<T> ToSpecSingleAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.Where(spec).SingleAsync(cancellationToken);
    }


    public static async Task<T?> ToSpecSingleOrDefaultAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.Where(spec).SingleOrDefaultAsync(cancellationToken);
    }

    public static List<T> ToSpecList<T>(this IQueryable<T> queryable, ISpecification<T> spec)
        where T : class
    {
        return [.. queryable.Where(spec)];
    }

    public static async Task<List<TModel>> ToSpecListAsync<TEntity,TModel>(this IQueryable<TEntity> queryable, ISpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TModel : new()
    {
        return await queryable.Where(spec)
            .ProjectToType<TEntity, TModel>()
            .ToListAsync(cancellationToken);
    }
    
    public static async Task<List<TEntity>> ToSpecListAsync<TEntity>(this IQueryable<TEntity> queryable, ISpecification<TEntity> spec,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        return await queryable.Where(spec).ToListAsync(cancellationToken);
    }


    public static int ToSpecCount<T>(this IQueryable<T> queryable, ISpecification<T> spec)
        where T : class
    {
        return queryable.Where(spec).Count();
    }

    public static async Task<int> ToSpecCountAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.Where(spec).CountAsync(cancellationToken);
    }


    public static bool ToSpecAny<T>(this IQueryable<T> queryable, ISpecification<T> spec)
        where T : class
    {
        return queryable.Where(spec).Any();
    }

    public static async Task<bool> ToSpecAnyAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.Where(spec).AnyAsync(cancellationToken);
    }


    public static bool ToSpecAll<T>(this IQueryable<T> queryable, ISpecification<T> spec)
        where T : class
    {
        return queryable.All(spec.IsSatisfiedBy());
    }

    public static async Task<bool> ToSpecAllAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return await queryable.AllAsync(spec.IsSatisfiedBy() , cancellationToken);
    }


    private static IQueryable<T> BaseWhere<T>(IQueryable<T> queryable, ISpecification<T> specification)
        where T : class
    {
        var queryableResultWithIncludes = specification.Includes
            .Aggregate(queryable.AsNoTracking(), (current, include) => current.Include(include));
        
        var secondaryResult = specification.IncludeStrings
            .Aggregate(queryableResultWithIncludes.AsNoTracking(),
                (current, include) => current.Include(include));
        
        return secondaryResult.Where(specification.IsSatisfiedBy());

        // return specification.RawQueries is not null 
        //     ? queryable.Specify(specification.RawQueries).Where(specification.IsSatisfiedBy())
        //     : queryable.Where(specification.IsSatisfiedBy());
    }

    // private static IQueryable<T> Specify<T>(this IQueryable<T> source,
    //     Func<IQueryable<T>, IQueryable<T>> builder)
    // {
    //     return builder(source);
    // }
}
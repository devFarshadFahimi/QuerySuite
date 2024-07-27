using Microsoft.EntityFrameworkCore;
using QuerySuite.Core.Contracts.SpecificationPattern.Extensions;

namespace QuerySuite.Core.Contracts.SpecificationPattern.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> queryable, ISpecification<T> specification)
            where T : class
                => BaseWhere(queryable, specification);

        public static async Task<T> FirstAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec, CancellationToken cancellationToken = default)
            where T : class
        {
            return await queryable.FirstAsync(spec.IsSatisfiedBy(), cancellationToken);
        }


        public static async Task<T?> FirstOrDefaultAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec, CancellationToken cancellationToken = default)
            where T : class
        {
            return await queryable.FirstOrDefaultAsync(spec.IsSatisfiedBy(), cancellationToken);
        }


        public static async Task<T> SingleAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec, CancellationToken cancellationToken = default)
            where T : class
        {
            return await queryable.SingleAsync(spec.IsSatisfiedBy(), cancellationToken);
        }


        public static async Task<T?> SingleOrDefaultAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec, CancellationToken cancellationToken = default)
            where T : class
        {
            return await queryable.SingleOrDefaultAsync(spec.IsSatisfiedBy(), cancellationToken);
        }

        public static List<T> ToList<T>(this IQueryable<T> queryable, ISpecification<T> spec)
            where T : class
        {
            return [.. queryable.Where(spec)];
        }

        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec, CancellationToken cancellationToken = default)
            where T : class
        {
            return await queryable.Where(spec).ToListAsync(cancellationToken);
        }


        public static int Count<T>(this IQueryable<T> queryable, ISpecification<T> spec)
            where T : class
        {
            return queryable.Where(spec).Count();
        }

        public static async Task<int> CountAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec, CancellationToken cancellationToken = default)
            where T : class
        {
            return await queryable.Where(spec).CountAsync(cancellationToken);
        }


        public static bool Any<T>(this IQueryable<T> queryable, ISpecification<T> spec)
            where T : class
        {
            return queryable.Where(spec).Any();
        }

        public static async Task<bool> AnyAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec, CancellationToken cancellationToken = default)
            where T : class
        {
            return await queryable.Where(spec).AnyAsync(cancellationToken);
        }



        public static bool All<T>(this IQueryable<T> queryable, ISpecification<T> spec)
            where T : class
        {
            return queryable.All(spec.IsSatisfiedBy());
        }

        public static async Task<bool> AllAsync<T>(this IQueryable<T> queryable, ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await queryable.AllAsync(spec.IsSatisfiedBy(), cancellationToken: cancellationToken);
        }


        private static IQueryable<T> BaseWhere<T>(IQueryable<T> queryable, ISpecification<T> specification)
            where T : class
        {
            var queryableResultWithIncludes = specification.Includes
              .Aggregate(queryable.AsNoTracking(), (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = specification.IncludeStrings
                .Aggregate(queryableResultWithIncludes.AsNoTracking(),
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult.Where(specification.IsSatisfiedByFunc()).AsQueryable();
        }


    }
}

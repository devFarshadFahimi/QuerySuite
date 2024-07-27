using System.Linq.Expressions;

namespace QuerySuite.Core.Contracts.SpecificationPattern
{
    /// <summary>
    /// NotSpecification&lt;TEntity&gt;
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NotSpecification&lt;TEntity&gt;"/> class.
    /// </remarks>
    /// <param name="specification">The specification.</param>
    public class NotSpecification<TEntity>(ISpecification<TEntity> specification) : Specification<TEntity>
    {
        private readonly ISpecification<TEntity> _specification = specification;

        ///// <summary>
        ///// Determines whether the specified candidate is satisfied by TEntity.
        ///// </summary>
        ///// <param name="candidate">The candidate.</param>
        ///// <returns>
        ///// 	<c>true</c> if [is satisfied by] [the specified candidate]; otherwise, <c>false</c>.
        ///// </returns>
        //public override bool IsSatisfiedBy(TEntity candidate)
        //{
        //    return !_specification.IsSatisfiedBy(candidate);
        //}

        public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
        {
            var isSatisfiedBy = _specification.IsSatisfiedBy();

            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.Not(isSatisfiedBy.Body), isSatisfiedBy.Parameters);
        }
    }
}
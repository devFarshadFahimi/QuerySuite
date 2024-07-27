using System.Linq.Expressions;

namespace QuerySuite.Core.Contracts.SpecificationPattern
{
    /// <summary>
    /// AndSpecification&lt;TEntity&gt;
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AndSpecification&lt;TEntity&gt;"/> class.
    /// </remarks>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    public class AndSpecification<TEntity>(ISpecification<TEntity> left, ISpecification<TEntity> right) : CompositeSpecification<TEntity>(left, right)
    {

        ///// <summary>
        ///// Determines whether the specified candidate is satisfied by TEntity.
        ///// </summary>
        ///// <param name="candidate">The candidate.</param>
        ///// <returns>
        ///// 	<c>true</c> if [is satisfied by] [the specified candidate]; otherwise, <c>false</c>.
        ///// </returns>
        //public override bool IsSatisfiedBy(TEntity candidate)
        //{
        //    return Left.IsSatisfiedBy(candidate) && Right.IsSatisfiedBy(candidate);
        //}

        public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
        {
            var leftExpression = Left.IsSatisfiedBy();
            var rightExpression = Right.IsSatisfiedBy();

            return leftExpression.Compose(rightExpression, Expression.AndAlso);
        }
    }
}
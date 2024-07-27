namespace QuerySuite.Core.Contracts.SpecificationPattern
{
    /// <summary>
    /// CompositeSpecification&lt;TEntity&gt;
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CompositeSpecification&lt;TEntity&gt;"/> class.
    /// </remarks>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    public abstract class CompositeSpecification<TEntity>(ISpecification<TEntity> left, ISpecification<TEntity> right) : Specification<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly ISpecification<TEntity> Left = left;

        /// <summary>
        /// 
        /// </summary>
        protected readonly ISpecification<TEntity> Right = right;
    }
}
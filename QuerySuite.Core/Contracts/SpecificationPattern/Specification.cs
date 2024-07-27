using System.Linq.Expressions;

namespace QuerySuite.Core.Contracts.SpecificationPattern
{
    /// <summary>
    /// Abstraction on <c>ISpecification</c> that supplies And, Or and Not.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class Specification<TEntity> : ISpecification<TEntity>
    {
        ///// <summary>
        ///// Determines whether the specified candidate is satisfied by TEntity.
        ///// </summary>
        ///// <param name="candidate">The candidate.</param>
        ///// <returns>
        ///// 	<c>true</c> if [is satisfied by] [the specified candidate]; otherwise, <c>false</c>.
        ///// </returns>
        //public abstract bool IsSatisfiedBy(TEntity candidate);

        /// <summary>
        /// generate compiled func of specification expression
        /// </summary>
        /// <returns>an expression that contains where clause we want</returns>
        public abstract Expression<Func<TEntity, bool>> IsSatisfiedBy();

        /// <summary>
        /// generate specification Func
        /// </summary>
        /// <returns>a compiled func that contains where clause we want</returns>
        public Func<TEntity, bool> IsSatisfiedByFunc()
        {
            return IsSatisfiedBy().Compile();
        }

        /// <summary>
        /// Ands the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns></returns>
        public ISpecification<TEntity> And(ISpecification<TEntity> specification)
        {
            return new AndSpecification<TEntity>(this, specification);
        }

        /// <summary>
        /// Ors the specified specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns></returns>
        public ISpecification<TEntity> Or(ISpecification<TEntity> specification)
        {
            return new OrSpecification<TEntity>(this, specification);
        }

        /// <summary>
        /// Nots this instance.
        /// </summary>
        /// <returns></returns>
        public ISpecification<TEntity> Not()
        {
            return new NotSpecification<TEntity>(this);
        }

        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // string-based includes allow for including children of children, e.g. Basket.Items.Product
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected void SetIncludes(ISpecification<TEntity> leftExpression, ISpecification<TEntity> rightExpression)
        {
            foreach (var item in leftExpression.Includes)
            {
                if (!Includes.Any(p => p.Body.ToString() == item.Body.ToString()))
                {
                    AddInclude(item);
                };
            }

            foreach (var item in rightExpression.Includes)
            {
                if (!Includes.Any(p => p.Body.ToString() == item.Body.ToString()))
                {
                    AddInclude(item);
                };
            }
        }
        protected void SetIncludes(ISpecification<TEntity> specification)
        {
            foreach (var item in specification.Includes)
            {
                if (!Includes.Any(p => p.Body.ToString() == item.Body.ToString()))
                {
                    AddInclude(item);
                };
            }
        }
    }
}
using System.Linq.Expressions;

namespace QuerySuite.Core.Contracts.SpecificationPattern;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>> IsSatisfiedBy();
    Func<TEntity, bool> IsSatisfiedByFunc();

    ISpecification<TEntity> And(ISpecification<TEntity> other);
    ISpecification<TEntity> Or(ISpecification<TEntity> other);
    ISpecification<TEntity> Not();

    // Func<IQueryable<TEntity>, IQueryable<TEntity>>? RawQueries { get; set; }
    List<Expression<Func<TEntity, object>>> OrderByDescending {get;}
    List<Expression<Func<TEntity, object>>> OrderBy { get; }
    List<Expression<Func<TEntity, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
}
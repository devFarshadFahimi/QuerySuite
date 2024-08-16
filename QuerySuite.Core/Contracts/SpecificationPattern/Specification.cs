using System.Linq.Expressions;

namespace QuerySuite.Core.Contracts.SpecificationPattern;

public abstract class Specification<TEntity> : ISpecification<TEntity>
{
    public abstract Expression<Func<TEntity, bool>> IsSatisfiedBy();

    public Func<TEntity, bool> IsSatisfiedByFunc()
    {
        return IsSatisfiedBy().Compile();
    }

    public ISpecification<TEntity> And(ISpecification<TEntity> specification)
    {
        return new AndSpecification<TEntity>(this, specification);
    }

    public ISpecification<TEntity> Or(ISpecification<TEntity> specification)
    {
        return new OrSpecification<TEntity>(this, specification);
    }

    public ISpecification<TEntity> Not()
    {
        return new NotSpecification<TEntity>(this);
    }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    public List<string> IncludeStrings { get; } = [];
    public List<Expression<Func<TEntity, object>>> OrderBy {get;} = [];
    public List<Expression<Func<TEntity, object>>> OrderByDescending {get;} = [];
    
    // public Func<IQueryable<TEntity>, IQueryable<TEntity>>? RawQueries { get; set; }

    // protected void AddRawQuery(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
    // {
    //     RawQueries = includes;
    // }
     
    protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
    {
        OrderByDescending.Add(orderByDescExpression);
    }
    
     
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy.Add(orderByExpression);
    }
    
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    // string-based includes allow for including children of children, e.g. Basket.Items.Product
    protected void AddInclude(params string[] relations)
    {
        IncludeStrings.Add(ConcatStringsWithDot(relations));
    }

    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }
        
    private static string ConcatStringsWithDot(params string[] types)
    {
        return string.Join(".", types);
    }
}
using System.Linq.Expressions;
using QuerySuite.Core.Contracts.SpecificationPattern;
using QuerySuite.Usage.Sample.Data;

namespace QuerySuite.Usage.Sample.Specs;

public class ByBookIsPublished : Specification<Book>
{
    private readonly bool _isPublished;
    public ByBookIsPublished(bool isPublished)
    {
        _isPublished = isPublished;
        AddOrderByDesc(p=>p.Id);
    }
    
    public override Expression<Func<Book, bool>> IsSatisfiedBy()
    {
        return p => p.IsPublished == _isPublished;
    }
}
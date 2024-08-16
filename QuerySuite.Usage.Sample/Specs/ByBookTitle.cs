using System.Linq.Expressions;
using QuerySuite.Core.Contracts.SpecificationPattern;
using QuerySuite.Usage.Sample.Data;

namespace QuerySuite.Usage.Sample.Specs;

public class ByBookTitle(string title) : Specification<Book>
{
    public override Expression<Func<Book, bool>> IsSatisfiedBy()
    {
        return p => p.Title.Contains(title);
    }
}
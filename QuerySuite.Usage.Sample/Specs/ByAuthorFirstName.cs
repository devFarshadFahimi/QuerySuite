using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuerySuite.Core.Contracts.SpecificationPattern;
using QuerySuite.Usage.Sample.Data;

namespace QuerySuite.Usage.Sample.Specs;

public class ByAuthorFirstName : Specification<Book>
{
    private readonly string _firstName;

    public ByAuthorFirstName(string firstName)
    {
        _firstName = firstName;
        AddInclude("Author", "AuthorContact");
        // AddRawQuery(p => p.Include(q => q.Author)
        //     .ThenInclude(q => q.AuthorContact));
    }

    public override Expression<Func<Book, bool>> IsSatisfiedBy()
    {
        return p => p.Author.FirstName.Contains(_firstName);
    }
}
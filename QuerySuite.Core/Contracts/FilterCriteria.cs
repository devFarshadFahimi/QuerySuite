using QuerySuite.Core.Enums;

namespace QuerySuite.Core.Contracts;
public class FilterCriteria
{
    public required string Column { get; set; }
    public required string Value { get; set; }
    public FilterCondition Condition { get; set; }
}
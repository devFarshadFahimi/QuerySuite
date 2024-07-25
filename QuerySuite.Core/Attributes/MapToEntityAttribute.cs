namespace QuerySuite.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapToEntityAttribute(string entityPropertyPath) : Attribute
{
    public string EntityPropertyPath { get; } = entityPropertyPath;
}

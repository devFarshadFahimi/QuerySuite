using System.Linq.Expressions;

namespace QuerySuite.Core.Contracts.SpecificationPattern
{
    public class ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map) : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (map.TryGetValue(p, out replacement!))
                p = replacement;

            return base.VisitParameter(p);
        }
    }
}
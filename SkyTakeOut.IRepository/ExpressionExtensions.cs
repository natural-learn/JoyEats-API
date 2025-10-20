namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) return right;
            if (right == null) return left;

            // 替换right表达式的参数为left的参数（避免参数实例不一致）
            var visitor = new ParameterRebinder(right.Parameters[0], left.Parameters[0]);
            var rightBody = visitor.Visit(right.Body);

            // 组合为 left && right
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left.Body, rightBody),
                left.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = left.Parameters[0];

            var body = Expression.OrElse(left.Body, RebindParameter(right.Body, right.Parameters[0], parameter));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression RebindParameter(Expression expression, ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            return new ParameterRebinder(oldParameter, newParameter).Visit(expression);
        }

        private class ParameterRebinder : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ParameterRebinder(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == _oldParameter)
                    return _newParameter;

                return base.VisitParameter(node);
            }
        }
    }
}

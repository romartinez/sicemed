using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class ReflectionHelper
    {
        public static MethodInfo GetMethod<T>(Expression<Func<T, object>> expression)
        {
            var methodCall = (MethodCallExpression) expression.Body;
            return methodCall.Method;
        }

        public static PropertyInfo GetProperty<TModel>(Expression<Func<TModel, object>> expression)
        {
            return (PropertyInfo) GetMember(expression);
        }

        public static MemberInfo GetMember<TModel>(Expression<Func<TModel, object>> expression)
        {
            var memberExpression = GetMemberExpression(expression);
            return memberExpression.Member;
        }

        public static FieldInfo GetField<TModel>(Expression<Func<TModel, object>> expression)
        {
            return (FieldInfo) GetMember(expression);
        }

        public static MethodInfo GetMethod<TModel>(Expression<Func<TModel, Action>> exp)
        {
            return GetMethodFromLambda(exp);
        }

        private static MethodInfo GetMethodFromLambda(LambdaExpression exp)
        {
            var unaryExp = (UnaryExpression) exp.Body;

            var methodCallExp = (MethodCallExpression) unaryExp.Operand;

            var constantExp = (ConstantExpression) methodCallExp.Arguments[2];

            var output = (MethodInfo) constantExp.Value;

            return output;
        }

        private static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression) expression.Body;
                memberExpression = body.Operand as MemberExpression;
            } else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }
            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "member");
            }
            return memberExpression;
        }
    }
}
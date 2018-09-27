using System;
using System.Linq.Expressions;

namespace Utilities
{
    public static class PropertyPathExpressionExtensions
    {
        public static string ParseAsPropertyPath<TObject>(this Expression<Func<TObject, object>> pathExpression)
        {
            var expressionString = pathExpression.Body.ToString();

            var propertyPath = IsValueProperty(expressionString) ?
                ParseValueProperty(expressionString) :
                ParseReferencePropety(expressionString);

            return propertyPath;
        }

        private static bool IsValueProperty(string expressionString) => expressionString.Contains(',');

        private static string ParseValueProperty(string expressionString)
        {
            var propertyNameBegin = expressionString.IndexOf('.') + 1;
            var propertyNameEnd = expressionString.IndexOf(',');
            var propertyNameLength = propertyNameEnd - propertyNameBegin;
            var propertyName = expressionString.Substring(propertyNameBegin, propertyNameLength);
            return propertyName;
        }

        private static string ParseReferencePropety(string expressionString)
        {
            var propertyNameBegin = expressionString.IndexOf('.') + 1;
            var propertyName = expressionString.Substring(propertyNameBegin);
            return propertyName;
        }
    }
}

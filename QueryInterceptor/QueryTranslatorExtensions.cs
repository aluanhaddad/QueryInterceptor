using System.Linq;
using System.Linq.Expressions;

namespace QueryInterceptor
{
    public static class QueryTranslatorExtensions
    {
        /// <summary>
        /// QueryInterceptor introduces one extension method on IQueryable (InterceptWith) that lets you plug in arbitrary expression visitors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="visitors">The visitors.</param>
        /// <returns>IQueryable{T}</returns>
        public static IQueryable<T> InterceptWith<T>(this IQueryable<T> source, params ExpressionVisitor[] visitors)
        {
            Check.NotNull(source, "source");
            Check.NotNull(visitors, "visitors");

            return new QueryTranslator<T>(source, visitors);
        }
    }
}
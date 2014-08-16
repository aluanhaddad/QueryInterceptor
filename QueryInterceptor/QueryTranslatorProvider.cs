using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace QueryInterceptor
{
    internal abstract class QueryTranslatorProvider : ExpressionVisitor
    {
        private readonly IQueryable _source;

        protected QueryTranslatorProvider(IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            _source = source;
        }

        internal IQueryable Source
        {
            get { return _source; }
        }
    }

    internal class QueryTranslatorProviderAsync : QueryTranslatorProvider, IDbAsyncQueryProvider
    {
        private readonly IEnumerable<ExpressionVisitor> _visitors;

        public QueryTranslatorProviderAsync(IQueryable source, IEnumerable<ExpressionVisitor> visitors)
            : base(source)
        {
            if (visitors == null)
            {
                throw new ArgumentNullException("visitors");
            }

            _visitors = visitors;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return new QueryTranslator<TElement>(Source, expression, _visitors);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            Type elementType = expression.Type.GetGenericArguments().First();
            return (IQueryable)Activator.CreateInstance(typeof(QueryTranslator<>).MakeGenericType(elementType), new object[] { Source, expression, _visitors });
        }

        public TResult Execute<TResult>(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return (TResult)(this as IQueryProvider).Execute(expression);
        }

        public object Execute(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var translated = VisitAll(expression);
            return Source.Provider.Execute(translated);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        internal IEnumerable ExecuteEnumerable(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var translated = VisitAll(expression);
            return Source.Provider.CreateQuery(translated);
        }

        private Expression VisitAll(Expression expression)
        {
            // Run all visitors in order
            var visitors = new ExpressionVisitor[] { this }.Concat(_visitors);

            return visitors.Aggregate(expression, (expr, visitor) => visitor.Visit(expr));
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            // Fix up the Expression tree to work with the underlying LINQ provider
            if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof(QueryTranslator<>))
            {
                var provider = ((IQueryable)node.Value).Provider as QueryTranslatorProvider;

                if (provider != null)
                {
                    return provider.Source.Expression;
                }

                return Source.Expression;
            }

            return base.VisitConstant(node);
        }
    }
}
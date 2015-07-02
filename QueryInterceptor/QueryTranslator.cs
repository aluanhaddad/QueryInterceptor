﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace QueryInterceptor
{
    internal class QueryTranslator : QueryTranslator<object>
    {
        public QueryTranslator(IQueryable source, IEnumerable<ExpressionVisitor> visitors) : base(source, visitors)
        {
        }

        public QueryTranslator(IQueryable source, Expression expression, IEnumerable<ExpressionVisitor> visitors) : base(source, expression, visitors)
        {
        }
    }

    internal class QueryTranslator<T> : IOrderedQueryable<T>, IDbAsyncEnumerable<T>
    {
        private readonly Expression _expression;
        private readonly QueryTranslatorProviderAsync _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTranslator{T}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="visitors">The visitors.</param>
        public QueryTranslator(IQueryable source, IEnumerable<ExpressionVisitor> visitors)
        {
            Check.NotNull(source, "source");

            // ReSharper disable PossibleMultipleEnumeration
            Check.NotNull(visitors, "visitors");

            _expression = Expression.Constant(this);
            _provider = new QueryTranslatorProviderAsync(source, visitors);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTranslator{T}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="visitors">The visitors.</param>
        public QueryTranslator(IQueryable source, Expression expression, IEnumerable<ExpressionVisitor> visitors)
        {
            Check.NotNull(source, "source");
            Check.NotNull(expression, "expression");

            // ReSharper disable PossibleMultipleEnumeration
            Check.NotNull(visitors, "visitors");

            _expression = expression;
            _provider = new QueryTranslatorProviderAsync(source, visitors);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_provider.ExecuteEnumerable(_expression)).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _provider.ExecuteEnumerable(_expression).GetEnumerator();
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
        /// </summary>
        /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
        public Type ElementType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
        /// </summary>
        /// <returns>The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of <see cref="T:System.Linq.IQueryable" />.</returns>
        public Expression Expression
        {
            get { return _expression; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.</returns>
        public IQueryProvider Provider
        {
            get { return _provider; }
        }

        /// <summary>
        /// Gets the asynchronous enumerator.
        /// </summary>
        /// <returns></returns>
        IDbAsyncEnumerator<T> IDbAsyncEnumerable<T>.GetAsyncEnumerator()
        {
            return ((IDbAsyncEnumerable<T>)_provider.ExecuteEnumerableAsync(_expression)).GetAsyncEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that can be used to asynchronously enumerate the sequence.
        /// </summary>
        /// <returns>
        /// Enumerator for asynchronous enumeration over the sequence.
        /// </returns>
        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return _provider.ExecuteEnumerableAsync(_expression).GetAsyncEnumerator();
        }
    }
}
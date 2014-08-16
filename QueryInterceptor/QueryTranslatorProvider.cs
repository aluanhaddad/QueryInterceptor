using System.Linq;
using System.Linq.Expressions;

namespace QueryInterceptor
{
    internal abstract class QueryTranslatorProvider : ExpressionVisitor
    {
        private readonly IQueryable _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTranslatorProvider"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        protected QueryTranslatorProvider(IQueryable source)
        {
            Check.NotNull(source, "source");

            _source = source;
        }

        internal IQueryable Source
        {
            get { return _source; }
        }
    }
}
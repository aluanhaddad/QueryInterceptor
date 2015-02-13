using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using QueryInterceptor;
using Xunit;

namespace QueryInterceptorAsyncUnitTests
{
    public class EqualsToNotEqualsVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Equal)
            {
                // Change == to !=
                return Expression.NotEqual(node.Left, node.Right);
            }

            return base.VisitBinary(node);
        }
    }

    public class Test
    {
        [Fact]
        public void TestEqualsToNotEqualsVisitor()
        {
            var query = Enumerable.Range(0, 10).AsQueryable().Where(n => n%2 == 0);
            var numbersEven = query.ToList();
            Assert.Equal(new List<int> {0, 2, 4, 6, 8}, numbersEven);

            var visitor = new EqualsToNotEqualsVisitor();
            var numbersOdd = query.InterceptWith(visitor).Where(x => x >= 0).ToList();
            Assert.Equal(new List<int> {1, 3, 5, 7, 9}, numbersOdd);
        }

        [Fact]
        public void TestAsync()
        {
            var queryEven = Enumerable.Range(0, 10).AsQueryable().Where(n => n%2 == 0);

            var visitor = new EqualsToNotEqualsVisitor();
            var queryOdd = queryEven.InterceptWith(visitor);

            var task = queryOdd.FirstAsync(n => n > 5, CancellationToken.None);
            task.ContinueWith(t => t.Result).Wait();

            Assert.Equal(7, task.Result);
        }
    }
}
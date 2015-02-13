using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using QueryInterceptor;

namespace QueryInterceptorAsyncCmdTest
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

    class Program
    {
        static void Main(string[] args)
        {
            TestEqualsToNotEqualsVisitor();
            TestAsync();
            TestDb();
        }


        private static void TestEqualsToNotEqualsVisitor()
        {
            Console.WriteLine("TestEqualsToNotEqualsVisitor");

            var query = Enumerable.Range(0, 10).AsQueryable().Where(n => n % 2 == 0);
            Console.WriteLine("Print even numbers");
            foreach (var item in query)
            {
                Console.Write(item + " ");
            }

            Console.WriteLine();

            Console.WriteLine("Print odd numbers");
            var visitor = new EqualsToNotEqualsVisitor();
            foreach (var item in query.InterceptWith(visitor).Where(x => x >= 0))
            {
                Console.WriteLine(item);
            }
        }

        private static void TestAsync()
        {
            Console.WriteLine("TestAsync");

            var query = Enumerable.Range(0, 10).AsQueryable().Where(n => n % 2 == 0);

            var visitor = new EqualsToNotEqualsVisitor();
            query = query.InterceptWith(visitor);

            var task = query.FirstAsync(n => n > 5, CancellationToken.None);
            task.ContinueWith(t => t.Result).Wait();

            Console.WriteLine("FirstAsync = '{0}'", task.Result);
        }

        private static void TestDb()
        {
            Console.WriteLine("TestDb");

            using (var context = GetContext())
            {
                var task1 = context.People.Where(p => p.Id % 2 == 0).FirstAsync();
                task1.ContinueWith(t => t.Result).Wait();

                Console.WriteLine("FirstAsync = '{0}'", task1.Result.Name);

                var visitor = new EqualsToNotEqualsVisitor();
                var query = context.People.InterceptWith(visitor).Where(p => p.Id % 2 == 0);

                var task2 = query.FirstAsync();
                task2.ContinueWith(t => t.Result).Wait();

                Console.WriteLine("FirstAsync [InterceptWith] = '{0}'", task2.Result.Name);
            }
        }

        private static TestDatabaseEntities GetContext()
        {
            TestDatabaseEntities ctx;

            try
            {
                ctx = new TestDatabaseEntities();
                ctx.Database.CommandTimeout = 5;
                ctx.People.FirstOrDefault();
                return ctx;
            }
            catch
            {
                ctx = new TestDatabaseEntities("name=TestDatabaseEntities_ProjectsV12");
                ctx.Database.CommandTimeout = 5;
                ctx.People.FirstOrDefault();
                return ctx;
            }
        }
    }
}
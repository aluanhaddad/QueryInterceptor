#if ASPNETCORE50
// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IDbAsyncQueryProvider
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 000F5452-2AD1-45BF-987B-3043022F9799
// Assembly location: C:\Users\Stef\Documents\Github\forks\QueryInterceptor\packages\EntityFramework.6.1.3\lib\net45\EntityFramework.dll

using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Defines methods to create and asynchronously execute queries that are described by an
  ///             <see cref="T:System.Linq.IQueryable"/> object.
  ///             This interface is used to interact with Entity Framework queries and shouldn't be implemented by custom classes.
  /// 
  /// </summary>
  public interface IDbAsyncQueryProvider : IQueryProvider
  {
    /// <summary>
    /// Asynchronously executes the query represented by a specified expression tree.
    /// 
    /// </summary>
    /// <param name="expression">An expression tree that represents a LINQ query. </param><param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for the task to complete.
    ///             </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    ///             The task result contains the value that results from executing the specified query.
    /// 
    /// </returns>
    Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously executes the strongly-typed query represented by a specified expression tree.
    /// 
    /// </summary>
    /// <typeparam name="TResult">The type of the value that results from executing the query. </typeparam><param name="expression">An expression tree that represents a LINQ query. </param><param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for the task to complete.
    ///             </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    ///             The task result contains the value that results from executing the specified query.
    /// 
    /// </returns>
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken);
  }
}
#endif
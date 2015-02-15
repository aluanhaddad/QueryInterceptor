#if ASPNETCORE50
using Microsoft.Data.Entity.Query;

namespace System.Data.Entity.Infrastructure
{
    /// <summary>
    /// Interface only used for ASPNETCORE50
    /// </summary>
    public interface IDbAsyncQueryProvider : IAsyncQueryProvider
    {
    }
}
#endif
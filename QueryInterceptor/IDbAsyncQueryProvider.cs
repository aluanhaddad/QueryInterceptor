#if ASPNETCORE50
using Microsoft.Data.Entity.Query;

namespace System.Data.Entity.Infrastructure
{
    /// <summary>
    /// Proxy interface to map IDbAsyncQueryProvider (for NET4.5 and ASPNET5) to IAsyncQueryProvider (used in ASPNETCORE5)
    /// </summary>
    public interface IDbAsyncQueryProvider : IAsyncQueryProvider
    {
    }
}
#endif
using Epa.Engine.DB;
using Microsoft.EntityFrameworkCore.Storage;

namespace Epa.Engine.Repository
{
    public class TransactionHandler : IDisposable
    {
        public IDbContextTransaction CurrentTransaction { get; set; } = null;
        public void Dispose()
        {
            CurrentTransaction = null;
        }
    }
}

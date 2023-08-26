using EPA.Engine.DB;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPA.Engine.Repository
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

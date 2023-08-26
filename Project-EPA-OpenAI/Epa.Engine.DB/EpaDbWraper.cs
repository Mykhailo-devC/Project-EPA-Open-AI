using EPA.Engine.DB;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPA.Engine.DB
{
    public class EpaDbWraper : IDisposable
    {
        public readonly EpaDbContext _context;
        protected EpaDbWraper(EpaDbContext context)
        {
            _context = context;
        }

        protected void Save()
        {
            _context.SaveChanges();
        }
        protected async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}

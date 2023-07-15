using Epa.Engine.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epa.Engine.Tests
{
    public class EpaRepositoryTests
    {
    }

    public interface IRepository
    {
        public void Get();
        public void Update(int id);
    }

    public class EpaDataAccess : EpaDbWraper, IRepository 
    {
        public EpaDataAccess(EpaDbContext context)
            : base(context)
        {
        }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public void Update(int id)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDataTransferObject
    {

    }

    public class EpaDbWraper : IDisposable
    {
        protected readonly EpaDbContext _context;
        public EpaDbWraper(EpaDbContext context)
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

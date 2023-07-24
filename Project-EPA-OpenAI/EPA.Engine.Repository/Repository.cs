using Epa.Engine.DB;
using Epa.Engine.Models;
using Epa.Engine.Models.DTO_Models;

namespace Epa.Engine.Repository
{
    public abstract class Repository
        : EpaDbWraper, IRepository
    {
        public Repository(EpaDbContext context)
            : base(context)
        {
        }
        abstract public Task<IQueryResult> Get(int id);
        abstract public Task<IQueryResult> GetAll();
        abstract public Task<IQueryResult> Add(DtoEntity item);
        abstract public Task<IQueryResult> Update(int id, DtoEntity item);
        abstract public Task<IQueryResult> Delete(int id);
    }
}

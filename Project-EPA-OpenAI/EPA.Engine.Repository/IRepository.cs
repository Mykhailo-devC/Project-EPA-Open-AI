using Epa.Engine.Models;
using Epa.Engine.Models.DTO_Models;

namespace Epa.Engine.Repository
{
    public interface IRepository
    {
        public Task<IQueryResult> Get(int id);
        public Task<IQueryResult> GetAll();
        public Task<IQueryResult> Add(DtoEntity item);
        public Task<IQueryResult> Update(int id, DtoEntity item);
        public Task<IQueryResult> Delete(int id);
    }
}

using Epa.Engine.Models;
using Epa.Engine.Models.DTO_Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Epa.Engine.Repository
{
    public interface IRepository
    {
        public Task<IQueryResult> Get(int id);
        public Task<IQueryResult> GetAll();
        public Task<IQueryResult> Add(DtoEntity item);
        public Task<IQueryResult> Update(int id, DtoEntity item);
        public Task<IQueryResult> Delete(int id, int listId);
        public Task<IQueryResult> Delete(int id);
    }

    public interface IRepositoryWrapper : IRepository
    {
        public Task<IQueryResult> Add(DtoEntity item, IDbContextTransaction transaction = null);
        public new Task<IQueryResult> Update(int id, DtoEntity item, IDbContextTransaction transaction = null);
    }
}

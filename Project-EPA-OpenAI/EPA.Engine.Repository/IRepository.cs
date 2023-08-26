using EPA.Engine.Models;
using EPA.Engine.Models.DTO_Models;
using EPA.Engine.Models.Result_Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPA.Engine.Repository
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
}

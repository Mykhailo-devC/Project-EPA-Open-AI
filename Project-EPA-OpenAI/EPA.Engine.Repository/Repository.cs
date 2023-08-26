using EPA.Engine.DB;
using EPA.Engine.Models;
using EPA.Engine.Models.DTO_Models;
using EPA.Engine.Models.Result_Models;

namespace EPA.Engine.Repository
{
    public abstract class Repository
        : EpaDbWraper, IRepository
    {
        public TransactionHandler _transactionHandler;
        public Repository(EpaDbContext context, TransactionHandler transactionHandler)
            : base(context)
        {
            _transactionHandler = transactionHandler;
        }

        protected void SetTransaction()
        {
            _transactionHandler.CurrentTransaction = _context.Database.CurrentTransaction;
        }

        abstract public Task<IQueryResult> Get(int id);
        abstract public Task<IQueryResult> GetAll();
        abstract public Task<IQueryResult> Add(DtoEntity item);
        abstract public Task<IQueryResult> Update(int id, DtoEntity item);
        abstract public Task<IQueryResult> Delete(int id, int listId);
        abstract public Task<IQueryResult> Delete(int id);
    }
}

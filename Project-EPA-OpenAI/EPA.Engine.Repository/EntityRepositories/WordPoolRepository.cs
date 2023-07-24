using Epa.Engine.DB;
using Epa.Engine.Models;
using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Entity_Models;

namespace Epa.Engine.Repository.EntityRepositories
{
    public class WordPoolRepository : Repository
    {
        public WordPoolRepository(EpaDbContext context) : base(context)
        {
        }

        public override Task<IQueryResult> Get(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<IQueryResult> GetAll()
        {
            throw new NotImplementedException();
        }

        public async override Task<IQueryResult> Add(DtoEntity item)
        {
            try
            {
                var dtoItem = (WordDTO)item;
                var newWord = new Word
                {
                    Value = dtoItem.Value,
                    WordList_Id = dtoItem.WordList_Id,
                };

                var result = await _context.WordPool.AddAsync(newWord);
                await SaveAsync();

                return new QueryResult<Word>
                {
                    Message = string.Format("New entity with id {0} was created in WordLsts table", result?.Entity.Id),
                    Success = true,
                    Result = new List<Word>() { result?.Entity }
                };
            }
            catch (Exception ex)
            {
                return new QueryResult<Word>
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                    Result = null
                };
            }
        }

        public async override Task<IQueryResult> Delete(int id)
        {
            try
            {
                var result = await _context.WordPool.FindAsync(id);

                if (result == null)
                {
                    return new QueryResult<Word>
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                        Result = null
                    };
                }

                _context.WordPool.Remove(result);
                await SaveAsync();

                return new QueryResult<Word>
                {
                    Message = string.Format("New entity with id {0} was updated in WordLsts table", result.Id),
                    Success = true,
                    Result = new List<Word>() { result }
                };
            }
            catch (Exception ex)
            {
                return new QueryResult<Word>
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                    Result = null
                };
            }
        }
        
        public async override Task<IQueryResult> Update(int id, DtoEntity item)
        {
            try
            {
                var dtoItem = (WordDTO)item;

                var result = await _context.WordPool.FindAsync(id);

                if (result == null)
                {
                    return new QueryResult<Word>
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                        Result = null
                    };
                }

                result.Value = dtoItem.Value;
                
                if(dtoItem.WordList_Id != default)
                {
                    result.WordList_Id = dtoItem.WordList_Id;
                }

                await SaveAsync();

                return new QueryResult<Word>
                {
                    Message = string.Format("New entity with id {0} was updated in WordLsts table", result.Id),
                    Success = true,
                    Result = new List<Word>() { result }
                };
            }
            catch (Exception ex)
            {
                return new QueryResult<Word>
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                    Result = null
                };
            }
        }
    }
}

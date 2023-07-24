using Epa.Engine.DB;
using Epa.Engine.Models;
using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Entity_Models;
using Microsoft.EntityFrameworkCore;

namespace Epa.Engine.Repository.EntityRepositories
{
    public class WordListRepository : Repository
    {
        public WordListRepository(EpaDbContext context) : base(context)
        {
        }

        public override async Task<IQueryResult> Get(int id)
        {
            try
            {
                var result = await _context.WordLists.Include(x => x.Words)
                                                     .Where(x => x.Id == id)
                                                     .FirstOrDefaultAsync();

                if (result == null)
                {
                    return new QueryResult<WordList>
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                        Result = null
                    };
                }

                return new QueryResult<WordList>
                {
                    Message = string.Format("Entity with id {0} was selected from WordLists table", result.Id),
                    Success = true,
                    Result = new List<WordList> { result }
                };
            }
            catch (Exception ex)
            {
                return new QueryResult<WordList>
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                    Result = null
                };
            }
        }

        public async override Task<IQueryResult> GetAll()
        {
            try
            {
                var result = await _context.WordLists.Include(x => x.Words).ToListAsync();

                return new QueryResult<WordList>
                {
                    Message = string.Format("{0} entities was selected from WordLists table", result.Count),
                    Success = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new QueryResult<WordList>
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                    Result = null
                };
            }
        }

        public async override Task<IQueryResult> Add(DtoEntity item)
        {
            try
            {
                var dtoItem = (WordListDTO)item;
                var newWordList = new WordList
                {
                    Name = dtoItem.Name
                };

                var result = await _context.WordLists.AddAsync(newWordList);
                await SaveAsync();

                return new QueryResult<WordList>
                {
                    Message = string.Format("New entity with id {0} was created in WordLsts table", result?.Entity.Id),
                    Success = true,
                    Result = new List<WordList>() { result?.Entity }
                };
            }
            catch (Exception ex)
            {
                return new QueryResult<WordList>
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
                var dtoItem = (WordListDTO)item;

                var result = await _context.WordLists.FindAsync(id);

                if (result == null)
                {
                    return new QueryResult<WordList>
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                        Result = null
                    };
                }

                result.Name = dtoItem.Name;
                await SaveAsync();

                return new QueryResult<WordList>
                {
                    Message = string.Format("New entity with id {0} was updated in WordLsts table", result.Id),
                    Success = true,
                    Result = new List<WordList>() { result }
                };
            }
            catch (Exception ex)
            {
                return new QueryResult<WordList>
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
                var result = await _context.WordLists.FindAsync(id);

                if(result == null)
                {
                    return new QueryResult<WordList>
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                        Result = null
                    };
                }

                _context.WordLists.Remove(result);
                await SaveAsync();

                return new QueryResult<WordList>
                {
                    Message = string.Format("New entity with id {0} was updated in WordLsts table", result.Id),
                    Success = true,
                    Result = new List<WordList>() { result }
                };
            }
            catch (Exception ex)
            {
                return new QueryResult<WordList>
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                    Result = null
                };
            }
        }
    }
}

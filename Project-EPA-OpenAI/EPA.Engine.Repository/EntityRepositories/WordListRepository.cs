using EPA.Engine.DB;
using EPA.Engine.Models;
using EPA.Engine.Models.DTO_Models;
using EPA.Engine.Models.Entity_Models;
using EPA.Engine.Models.Logic_Models;
using EPA.Engine.Models.Result_Models;
using Microsoft.EntityFrameworkCore;

namespace EPA.Engine.Repository.EntityRepositories
{
    public class WordListRepository : Repository
    {
        private readonly IRepository _wordPoolRepos;
        public WordListRepository(EpaDbContext context, TransactionHandler transactionHandler, ServiceResolver.RepositoryResolver accessor)
            : base(context, transactionHandler)
        {
            _wordPoolRepos = accessor(RepositoryType.WordPool);
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
                    return new QueryResult
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
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
                return new QueryResult
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
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
                return new QueryResult
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                };
            }
        }

        public async override Task<IQueryResult> Add(DtoEntity item)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                SetTransaction();

                var dtoWordList = (WordListDTO)item;
                var newWordList = new WordList
                {
                    Name = dtoWordList.Name
                };

                var result = await _context.WordLists.AddAsync(newWordList);
                await SaveAsync();

                if(dtoWordList.Words != null & dtoWordList.Words.Count !=0)
                {

                    foreach (var word in dtoWordList.Words)
                    {
                        await _wordPoolRepos.Add(new WordDTO { Value = word, WordList_Id = newWordList.Id });
                    }

                    await _context.WordListWords.Where(x => x.WordList_Id == result.Entity.Id)
                                                .Include(x => x.Word)
                                                .LoadAsync();

                    await _context.Database.CommitTransactionAsync();
                    return new QueryResult<WordList>
                    {
                        Message = string.Format("New entity with id {0} was created in WordLists table, with {1} words", newWordList.Id, dtoWordList.Words.Count),
                        Success = true,
                        Result = new List<WordList>() { newWordList }
                    };
                }
                else
                {
                    await _context.Database.CommitTransactionAsync();
                    return new QueryResult<WordList>
                    {
                        Message = string.Format("New empty entity with id {0} was created in WordLists table", newWordList),
                        Success = true,
                        Result = new List<WordList>() { newWordList }
                    };
                }
                
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                return new QueryResult
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                };
            }
            finally
            {
                _transactionHandler.Dispose();
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
                    return new QueryResult
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                    };
                }

                result.Name = dtoItem.Name;
                await SaveAsync();

                return new QueryResult<WordList>
                {
                    Message = string.Format("New entity with id {0} was updated in WordLists table", result.Id),
                    Success = true,
                    Result = new List<WordList>() { result }
                };
            }
            catch (Exception ex)
            {
                return new QueryResult
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                };
            }
        }

        public async override Task<IQueryResult> Delete(int id, int listId)
        {
            return await Delete(id);
        }

        public async override Task<IQueryResult> Delete(int id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                SetTransaction();

                var wordList = await _context.WordLists.Include(x => x.Words)
                                                     .FirstOrDefaultAsync(x => x.Id == id);

                if(wordList == null)
                {
                    await _context.Database.RollbackTransactionAsync();
                    return new QueryResult
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                    };
                }

                _context.WordLists.Remove(wordList);
                foreach (var word in wordList.Words)
                {
                    await _wordPoolRepos.Delete(word.Id, wordList.Id);
                }

                await SaveAsync();
                await _context.Database.CommitTransactionAsync();
                return new QueryResult<WordList>
                {
                    Message = string.Format("Entity with id {0} was deleted from WordLists table", wordList.Id),
                    Success = true,
                    Result = new List<WordList>() { wordList }
                };
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                return new QueryResult
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                };
            }
        }
    }
}

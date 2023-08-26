using EPA.Engine.DB;
using EPA.Engine.Models;
using EPA.Engine.Models.DTO_Models;
using EPA.Engine.Models.Entity_Models;
using EPA.Engine.Models.Result_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPA.Engine.Repository.EntityRepositories
{
    public class WordPoolRepository : Repository
    {
        public WordPoolRepository(EpaDbContext context, TransactionHandler transactionHandler)
            : base(context, transactionHandler)
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
                if(_transactionHandler.CurrentTransaction == null)
                {
                    await _context.Database.BeginTransactionAsync();
                }
                else
                {
                    await _context.Database.UseTransactionAsync(_transactionHandler.CurrentTransaction.GetDbTransaction(),
                                                                _transactionHandler.CurrentTransaction.TransactionId);
                }

                var dtoItem = (WordDTO)item;

                var word = await _context.WordPool.FirstOrDefaultAsync(x => x.Value == dtoItem.Value);
                if (word == null)
                {
                    word = new Word
                    {
                        Value = dtoItem.Value,
                    };
                    var result = await _context.WordPool.AddAsync(word);
                    await SaveAsync();
                }

                var wordMatching = new WordListWord()
                {
                    WordList_Id = dtoItem.WordList_Id,
                    Word_Id = word.Id
                };

                await _context.WordListWords.AddAsync(wordMatching);
                await SaveAsync();
                
                if(_transactionHandler.CurrentTransaction == null)
                {
                    await _context.Database.CommitTransactionAsync();
                }

                return new QueryResult<Word>
                {
                    Message = string.Format("New entity with id {0} was created in WordPool table", word.Id),
                    Success = true,
                    Result = new List<Word>() { word }
                };
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
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
                await _context.Database.BeginTransactionAsync();
                SetTransaction();

                var dtoItem = (WordDTO)item;
                var word = await _context.WordPool.Include(x => x.WordLists).FirstOrDefaultAsync(x => x.Id == id);

                if (word == null)
                {
                    return new QueryResult<Word>
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                        Result = null
                    };
                }

                if (word.WordLists.Count() == 1)
                {
                    word.Value = dtoItem.Value;

                    await SaveAsync();
                    await _context.Database.CommitTransactionAsync();
                    return new QueryResult<Word>
                    {
                        Message = string.Format("Entity with id {0} was updated in WordPool table", word.Id),
                        Success = true,
                        Result = new List<Word>() { word }
                    };
                }
                else
                {
                    var result = await Add(item);
                    var wordmatching = _context.WordListWords.Where(x => x.WordList_Id == dtoItem.WordList_Id && x.Word_Id == word.Id)
                                      .FirstOrDefault();
                    _context.WordListWords.Remove(wordmatching);
                    await SaveAsync();
                    await _context.Database.CommitTransactionAsync();

                    return new QueryResult<Word>
                    {
                        Message = string.Format("New entity with id {0} was updated in WordPool table", result.Result.Cast<Word>().FirstOrDefault().Id),
                        Success = true,
                        Result = new List<Word>() { result.Result.FirstOrDefault() as Word }
                    };
                }
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                return new QueryResult<Word>
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                    Result = null
                };
            }
        }

        public async override Task<IQueryResult> Delete(int id, int listId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var word = await _context.WordPool.FindAsync(id);

                if (word == null)
                {
                    return new QueryResult<Word>
                    {
                        Message = $"Entity with id {id} doesn't exist.",
                        Success = false,
                        Result = null
                    };
                }

                var wordMatchingList = await _context.WordListWords.Where(x => x.Word_Id == id).ToListAsync();

                if (listId != default && (wordMatchingList != null || wordMatchingList.Count != 0))
                {
                    var wordMatches = wordMatchingList.Where(x => x.WordList_Id == listId).ToList();
                    _context.WordListWords.RemoveRange(wordMatches);
                    await SaveAsync();

                    if (!await _context.WordListWords.AnyAsync(x => x.Word_Id == id))
                    {
                        _context.WordPool.Remove(word);
                    }

                    await SaveAsync();
                    await _context.Database.CommitTransactionAsync();

                    return new QueryResult<Word>
                    {
                        Message = string.Format("Word with id {0} and WordList with id {1}, " +
                                                "where no more connected by WordListWord entities.", word.Id, listId),
                        Success = true,
                        Result = new List<Word>() { word }
                    };
                }
                else
                {
                    _context.WordPool.Remove(word);
                    await SaveAsync();
                    await _context.Database.CommitTransactionAsync();

                    return new QueryResult<Word>
                    {
                        Message = string.Format("Entity with id {0} was deleted from WordPool table", word.Id),
                        Success = true,
                        Result = new List<Word>() { word }
                    };
                }
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
            return await Delete(id, default);
        }
    }
}

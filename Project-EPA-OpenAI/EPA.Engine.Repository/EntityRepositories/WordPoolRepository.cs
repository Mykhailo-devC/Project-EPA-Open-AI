using Epa.Engine.DB;
using Epa.Engine.Models;
using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Entity_Models;
using Microsoft.EntityFrameworkCore;

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
                await _context.Database.BeginTransactionAsync();
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
                await _context.Database.CommitTransactionAsync();
                return new QueryResult<Word>
                {
                    Message = string.Format("New entity with id {0} was created in WordLits table", word.Id),
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
                        Message = string.Format("Entity with id {0} was deleted from WordLits table", word.Id),
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

        public async override Task<IQueryResult> Update(int id, DtoEntity item)
        {
            try
            {
                var dtoItem = (WordDTO)item;
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

                word.Value = dtoItem.Value;

                await SaveAsync();

                return new QueryResult<Word>
                {
                    Message = string.Format("New entity with id {0} was updated in WordLits table", word.Id),
                    Success = true,
                    Result = new List<Word>() { word }
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

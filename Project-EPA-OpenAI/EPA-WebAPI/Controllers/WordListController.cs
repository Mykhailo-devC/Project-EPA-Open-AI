using Epa.Engine.DB;
using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Entity_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EPA_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordListController : ControllerBase
    {
        private readonly EpaDbContext _dbContext;

        public WordListController(EpaDbContext epaDb)
        {
            _dbContext = epaDb;
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok(_dbContext.GetType().Assembly.GetName().Name);
        }
        [HttpGet]
        public async Task<IEnumerable<WordList>> Get()
        {
            return await _dbContext.WordLists.Include(x => x.Words).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(WordListDTO wordListDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wordList = new WordList()
            {
                Name = wordListDto.Name,
            };

            await _dbContext.AddAsync(wordList);
            await _dbContext.SaveChangesAsync();

            return Ok(wordList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WordListDTO wordListDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wordList = await _dbContext.WordLists.FindAsync(id);

            if (wordList == null)
            {
                return NotFound($"No word list with id: {id}");
            }

            wordList.Name = wordListDto.Name;

            await _dbContext.SaveChangesAsync();

            return Ok(wordList);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var wordList = await _dbContext.WordLists.FindAsync(id);

            if(wordList == null)
            {
                return NotFound($"No word list with id: {id}");
            }

            _dbContext.WordLists.Remove(wordList);
            await _dbContext.SaveChangesAsync();

            return Ok(wordList);
        }
    }
}

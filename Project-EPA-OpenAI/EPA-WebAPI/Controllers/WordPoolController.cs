using Epa.Engine.DB;
using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Entity_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EPA_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordPoolController : ControllerBase
    {
        private readonly EpaDbContext _dbContext;

        public WordPoolController(EpaDbContext epaDb)
        {
            _dbContext = epaDb;
        }

        [HttpGet]
        public async Task<IEnumerable<Word>> Get()
        {
            return await _dbContext.wordPool.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(WordDTO wordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var word = new Word()
            {
                Value = wordDto.Value,
                WordList_Id = wordDto.WordList_Id
            };

            await _dbContext.AddAsync(word);
            await _dbContext.SaveChangesAsync();

            return Ok(word);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var word = await _dbContext.wordPool.FindAsync(id);

            if (word == null)
            {
                return NotFound($"No word with id: {id}");
            }

            _dbContext.wordPool.Remove(word);
            await _dbContext.SaveChangesAsync();

            return Ok(word);
        }
    }
}

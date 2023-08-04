using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Logic_Models;
using Epa.Engine.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EPA_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordPoolController : ControllerBase
    {
        private readonly IRepository _repository;

        public WordPoolController(ServiceResolver.RepositoryResolver accessor)
        {
            _repository = accessor(RepositoryType.WordPool);
        }

        [HttpPost]
        public async Task<IActionResult> Post(WordDTO wordDto)
        {
            if (!ModelState.IsValid & wordDto.WordList_Id == default)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.Add(wordDto);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WordDTO wordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.Update(id, wordDto);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery]int listId = default)
        {
            var result = await _repository.Delete(id, listId);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }

            return Ok(result);
        }
    }
}

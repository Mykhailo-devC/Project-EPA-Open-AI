using Epa.Engine.Models.DTO_Models;
using Epa.Engine.Models.Logic_Models;
using Epa.Engine.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EPA_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordListController : ControllerBase
    {
        private readonly IRepository _repository;

        public WordListController(ServiceResolver.RepositoryResolver accessor)
        {
            _repository = accessor(RepositoryType.WordList);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAll();

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(WordListDTO wordListDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.Add(wordListDto);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WordListDTO wordListDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _repository.Update(id, wordListDto);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.Delete(id);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }

            return Ok(result);
        }
    }
}

using EPA.OpenAI.Core.Services;
using EPA.OpenAI.Models.DTO_Models;
using Microsoft.AspNetCore.Mvc;

namespace EPA_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAISession _session;
        public OpenAIController(IOpenAISession session)
        {
            _session = session;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartSession(StartSessionDTO startSessionDTO)
        {
            var result = await _session.StartSession(startSessionDTO);

            if(!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckSentence(UserAnswerDTO answerDTO)
        {
            var result = await _session.CheckSentence(answerDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}

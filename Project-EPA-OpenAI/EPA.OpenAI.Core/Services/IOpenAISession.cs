using EPA.OpenAI.Models.DTO_Models;
using EPA.OpenAI.Models.Result_Models;

namespace EPA.OpenAI.Core.Services
{
    public interface IOpenAISession
    {
        Task<IOpenAIResult> StartSession(StartSessionDTO userStartSessionDTO);
        Task<IOpenAIResult> CheckSentence(UserAnswerDTO userAnswerDTO);
    }
}
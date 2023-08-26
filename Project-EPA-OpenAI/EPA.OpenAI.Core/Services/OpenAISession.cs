using EPA.Engine.Models.Entity_Models;
using EPA.Engine.Models.Logic_Models;
using EPA.Engine.Repository;
using EPA.OpenAI.Models.DTO_Models;
using EPA.OpenAI.Models.Logic_Models;
using EPA.OpenAI.Models.Result_Models;
using System.Net;

namespace EPA.OpenAI.Core.Services
{
    public class OpenAISession : IOpenAISession
    {
        private readonly IOpenAIService _AIService;
        private readonly IRepository _wordLists;
        private readonly SessionHandler _sessionHandler;
        public OpenAISession(IOpenAIService aIService, ServiceResolver.RepositoryResolver accessor, SessionHandler sessionHandler)
        {
            _AIService = aIService;
            _wordLists = accessor(RepositoryType.WordList);
            _sessionHandler = sessionHandler;
        }
        public async Task<IOpenAIResult> StartSession(StartSessionDTO userStartSessionDTO)
        {
            try
            {
                var currentWordList = await _wordLists.Get(userStartSessionDTO.WordList_id);
                if (!currentWordList.Success)
                {
                    return new OpenAIResult
                    {
                        Success = currentWordList.Success,
                        Message = currentWordList.Message,
                    };
                }

                var sentences = await _AIService.GenerateSentencesFromWordList(currentWordList
                                                                        .Result
                                                                        .Cast<WordList>()
                                                                        .First()
                                                                        .Words
                                                                        .Select(x => x.Value));

                var currentSession = _sessionHandler.NewSession(sentences, userStartSessionDTO.UserName, _AIService.CurrentConversation);
                return new OpenAIResult<SentenceData>
                {
                    Message = string.Format("Starting session {0}.\t{1} sentences was generated", currentSession.Id, sentences.Count()),
                    Success = true,
                    Result = sentences,
                    SessionId = currentSession.Id
                };
            }
            catch (Exception ex)
            {
                return new OpenAIResult
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                };
            }
        }

        public async Task<IOpenAIResult> CheckSentence(UserAnswerDTO answerDTO)
        {
            try
            {
                var session = _sessionHandler.FindSession(answerDTO.SessionId, answerDTO.UserName);

                if (session == null)
                {
                    return new OpenAIResult
                    {
                        Success = false,
                        Message = string.Format("Session with id {0} does not exists", answerDTO.SessionId)
                    };
                }

                _AIService.UseConversation(session.Conversation);
                var responce = await _AIService.AnalyzeSentenceAnswer(session.Sentences.Pop(), answerDTO.Answer);

                return new OpenAIResult<SentenceAnalyzeResult>
                {
                    Message = string.Format("Session id: {0} | Sentence #{1} was analyzed successfully, {2} words was affected.", session.Id, session.Sentences.Count, responce.Result.Count()),
                    Success = true,
                    Result = new List<SentenceAnalyzeResult>() { responce },
                    SessionId = session.Id
                };
            }
            catch (Exception ex)
            {
                return new OpenAIResult
                {
                    Message = $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    Success = false,
                };
            }
        }
    }
}
using EPA.OpenAI.Models.Logic_Models;

namespace EPA.OpenAI.Core.Services
{
    public interface IOpenAIService
    {
        ConversationHandler CurrentConversation { get; }
        Task<IEnumerable<SentenceData>> GenerateSentencesFromWordList(IEnumerable<string> words);
        Task<SentenceAnalyzeResult> AnalyzeSentenceAnswer(SentenceData sentence, string answer);
        void UseConversation(ConversationHandler conversationHandler);
    }
}
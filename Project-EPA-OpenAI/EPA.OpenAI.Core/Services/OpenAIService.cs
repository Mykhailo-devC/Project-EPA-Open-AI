using EPA.OpenAI.Models.Logic_Models;
using Newtonsoft.Json;

namespace EPA.OpenAI.Core.Services
{
    public class OpenAIService : IOpenAIService
    {
        private ConversationHandler Conversation;
        public ConversationHandler CurrentConversation { get { return Conversation; } }
        private OpenAIApiHandler OpenAIApiHandler { get; set; }
        public OpenAIService(OpenAIApiHandler openAIApi)
        {
            OpenAIApiHandler = openAIApi;
        }

        private void InitConversation()
        {
            Conversation = new ConversationHandler(OpenAIApiHandler);
        }

        public void UseConversation(ConversationHandler conversationHandler)
        {
            Conversation = conversationHandler;
        }

        public async Task<IEnumerable<SentenceData>> GenerateSentencesFromWordList(IEnumerable<string> words)
        {
            InitConversation();
            Conversation
                .GeneratingSentencesConversation
                .AppendUserInput(PromptGenerator
                .CreatePromptForGeneratingSentences(words));

            var jsonResponse = await Conversation.GeneratingSentencesConversation.GetResponseFromChatbotAsync();

            var deserializedSentences = JsonConvert.DeserializeObject<GeneratedSentences>(jsonResponse.ToString());
            return deserializedSentences.Sentences;
        }

        public async Task<SentenceAnalyzeResult> AnalyzeSentenceAnswer(SentenceData sentence, string answer)
        {
            Conversation
                .AnalyzingSentecesConversation
                .AppendUserInput(PromptGenerator
                .CreatePromptForAnalyzingSentence(sentence.Sentence, answer, sentence.Context));

            var jsonResponse = await Conversation.AnalyzingSentecesConversation.GetResponseFromChatbotAsync();

            var deserializedSentences = JsonConvert.DeserializeObject<SentenceAnalyzeResult>(jsonResponse.ToString());
            return deserializedSentences;
        }
    }
}
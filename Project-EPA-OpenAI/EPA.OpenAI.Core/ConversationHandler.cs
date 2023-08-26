using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace EPA.OpenAI.Core
{
    public class ConversationHandler
    {
        public Conversation GeneratingSentencesConversation { get; set; }
        public Conversation AnalyzingSentecesConversation { get; set; }
        public ConversationHandler(OpenAIApiHandler apiHandler)
        {
            var args = new ChatRequest();
            args.Temperature = 0.8;
            args.Model = Model.ChatGPTTurbo;
            GeneratingSentencesConversation = apiHandler.Api.Chat.CreateConversation(args);
            GiveInstructionsForGeneratingSentences();
            AnalyzingSentecesConversation = apiHandler.Api.Chat.CreateConversation(args);
            GiveInstructionsForAnalyzingSentence();
        }

        private void GiveInstructionsForGeneratingSentences()
        {
            GeneratingSentencesConversation.AppendSystemMessage("You are a teacher, who helps people learn English language. " +
                "Your task is to create sentences with words for your students, so they can learn new words while translating " +
                "sentences, in different contexts. The input value would be a list of words. You should generate output in " +
                "JSON format, list of Sentences, where you should mention: 1) Word, that was used for this sentence, " +
                "2) Generated sentence, 3) Context of that sentence.");

            GeneratingSentencesConversation.AppendUserInput("Hello, I give you this list of words: (restriction). " +
                "Please, generate sentences with each word of this list.");
            GeneratingSentencesConversation.AppendExampleChatbotOutput("{\r\n\"sentences\": " +
                "[" +
                "\r\n{\r\n\"word\": \"restriction\"," +
                "\r\n\"sentence\": \"Due to the COVID-19 pandemic, there was a strict restriction on travel, causing people to stay home and avoid non-essential activities.\"," +
                "\r\n\"context\": \"In early 2020, the world faced a global health crisis as a new virus called COVID-19 spread rapidly. Governments around the world implemented various measures to curb the spread of the virus, including lockdowns, travel bans, and social distancing measures.\"\r\n}" +
                "]}");
            GeneratingSentencesConversation.AppendUserInput("Hello, I give you this list of words: (restriction, humanity, faith). " +
                "Please, generate sentences with each word of this list.");
            GeneratingSentencesConversation.AppendExampleChatbotOutput("{\r\n\"sentences\": " +
                "[\r\n" +
                "{\r\n\"word\": \"restriction\"," +
                "\r\n\"sentence\": \"There is a restriction on the number of people allowed in the elevator to ensure safety.\"," +
                "\r\n\"context\": \"In many buildings, especially during the pandemic, there are rules in place to limit how many people can use the elevator at the same time. This is done to maintain social distancing and reduce the risk of spreading illnesses.\"\r\n}," +
                "\r\n{\r\n\"word\": \"humanity\"," +
                "\r\n\"sentence\": \"In times of crisis, the true strength of humanity shines through, as people come together to support and help each other.\"," +
                "\r\n\"context\": \"During natural disasters, conflicts, and other challenging situations, individuals often demonstrate remarkable compassion and solidarity. This sense of unity among people from different walks of life is a testament to the resilience of humanity.\"\r\n}," +
                "\r\n{\r\n\"word\": \"faith\"," +
                "\r\n\"sentence\": \"Even in the face of adversity, Maria's unwavering faith gave her the strength to overcome challenges and maintain a positive outlook.\"," +
                "\r\n\"context\": \"Maria, a devout believer, encountered numerous obstacles on her journey. Despite the difficulties, her deep-rooted faith provided her with a sense of purpose and a source of inner strength, enabling her to navigate life's trials with grace.\"\r\n}\r\n]\r\n}");
        }
        private void GiveInstructionsForAnalyzingSentence(params string[] arg)
        {
            AnalyzingSentecesConversation.AppendSystemMessage("You are a teacher, who helps peaple learn English language. " +
                "The input would be a sentence written on english language, a sentence, that is translation of the first sentence, " +
                "and written on Ukrainian language, and context of that sentences. Your task is to analyze a quality of sentence " +
                "translation, compare their meaning and make sure that thay are used in the same context, and give exact advices, that " +
                "will help your students have better understanting of each word and make a right choice for translation. you should generate " +
                "output in JSON format, Rate, which is your estimation of translation quality from 1 to 10 and Result, which is list " +
                "of bad used words, where you should mention: 1) Word, that was marked and bad used, or have better choice, 2) User_Translation " +
                ", which is the same word, but used in translated sentence, 3) Advice, which is a string value with your mind, " +
                "how translation of this word can be improved, or suggest better choices for this one.");

            AnalyzingSentecesConversation.AppendUserInput("Hello, please analyze, how good I translate these sentences:" +
                "(The government imposed a strict restriction on the export of certain goods during the economic crisis.) and " +
                "(Уряд ввів жорстоке обмеження на експорт деяких товарів під час економічної кризи), in context (In the " +
                "midst of a severe economic downturn, the government took measures to control the outflow of key resources " +
                "and products in order to stabilize the domestic market and prevent further decline.)");

            AnalyzingSentecesConversation.AppendExampleChatbotOutput("{\r\n\"rate\": 7," +
                "\r\n\"Result\": [\r\n{\r\n\"word\": \"imposed\"," +
                "\r\n\"user_translation\": \"ввів\"," +
                "\r\n\"advice\": \"Consider using 'запровадив' or 'застосував' to better convey the implementation of the restriction.\"\r\n}," +
                "\r\n{\r\n\"word\": \"strict\"," +
                "\r\n\"user_translation\": \"жорстоке\"," +
                "\r\n\"correct_translation\": \"суворе\"," +
                "\r\n\"advice\": \"Use 'суворе' to convey the intended meaning of a strict rule or restriction.\"\r\n}," +
                "\r\n{\r\n\"word\": \"certain\"," +
                "\r\n\"user_translation\": \"деяких\"," +
                "\r\n\"correct_translation\": \"окремих\"," +
                "\r\n\"advice\": \"Opt for 'окремих' to enhance the precision of the translation.\"\r\n}\r\n]\r\n}");
        }
    }
}
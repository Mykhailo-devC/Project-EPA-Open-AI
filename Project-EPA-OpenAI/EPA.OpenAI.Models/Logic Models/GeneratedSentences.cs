namespace EPA.OpenAI.Models.Logic_Models
{
    public class GeneratedSentences : OpenAIResponse
    {
        public IEnumerable<SentenceData> Sentences { get; set; }
    }
}
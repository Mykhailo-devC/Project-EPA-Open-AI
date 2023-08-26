namespace EPA.OpenAI.Models.Logic_Models
{
    public class SentenceData : OpenAIResponse
    {
        public string Word { get; set; }
        public string Sentence { get; set; }
        public string Context { get; set; }
    }
}
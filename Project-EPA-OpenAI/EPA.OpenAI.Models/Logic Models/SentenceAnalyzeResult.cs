namespace EPA.OpenAI.Models.Logic_Models
{
    public class SentenceAnalyzeResult : OpenAIResponse
    {
        public int Rate { get; set; }
        public virtual IEnumerable<WordAnalyzeResult> Result { get; set; }
    }
}

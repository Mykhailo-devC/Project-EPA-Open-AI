namespace EPA.OpenAI.Models.Logic_Models
{
    public class WordAnalyzeResult : OpenAIResponse
    {
        public string Word { get; set; }
        public string User_Translation { get; set; }
        public string Advice { get; set; }
    }
}

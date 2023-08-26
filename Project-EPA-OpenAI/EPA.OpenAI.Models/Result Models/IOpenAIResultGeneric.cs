namespace EPA.OpenAI.Models.Result_Models
{
    public interface IOpenAIResult<T> : IOpenAIResult where T : OpenAIResponse
    {
        new public IEnumerable<T> Result { get; set; }
    }
}

using EPA.Common;

namespace EPA.OpenAI.Models.Result_Models
{
    public class OpenAIResult : IOpenAIResult
    {
        public IEnumerable<OpenAIResponse> Result { get; set; } = null;
        public string Message { get; set; }
        public bool Success { get; set; }
        public Guid SessionId { get; set; } = Guid.Empty;
        IEnumerable<object> IResult.Result { get => Result; set => Result = value as IEnumerable<OpenAIResponse>; }
    }
}

using EPA.Common;

namespace EPA.OpenAI.Models.Result_Models
{
    public class OpenAIResult<T> : IOpenAIResult<T> where T : OpenAIResponse
    {
        public IEnumerable<T> Result { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public Guid SessionId { get; set; }
        IEnumerable<object> IResult.Result { get => Result; set => Result = value as IEnumerable<T>; }
        IEnumerable<OpenAIResponse> IOpenAIResult.Result { get => Result; set => Result = value as IEnumerable<T>; }
    }
}

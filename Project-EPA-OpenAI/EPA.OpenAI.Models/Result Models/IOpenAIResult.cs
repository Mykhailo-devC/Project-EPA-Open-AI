using EPA.Common;

namespace EPA.OpenAI.Models.Result_Models
{
    public interface IOpenAIResult : IResult
    {
        public Guid SessionId { get; set; }
        new public IEnumerable<OpenAIResponse> Result { get; set; }
    }

    
}

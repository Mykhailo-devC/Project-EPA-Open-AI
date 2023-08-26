using EPA.Common;

namespace EPA.Engine.Models.Result_Models
{
    public class QueryResult : IQueryResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public IEnumerable<Entity> Result { get; set; } = null;
        IEnumerable<object> IResult.Result { get => Result; set => Result = value as IEnumerable<Entity>; }
    }
}

using EPA.Common;

namespace EPA.Engine.Models.Result_Models
{
    public interface IQueryResult : IResult
    {
        new public IEnumerable<Entity> Result { get; set; }
    }
}

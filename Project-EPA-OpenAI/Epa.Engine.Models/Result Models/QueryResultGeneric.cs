using EPA.Common;

namespace EPA.Engine.Models.Result_Models
{
    public class QueryResult<TEntity> : IQueryResult<TEntity> where TEntity : Entity
    {
        public IEnumerable<TEntity> Result { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        IEnumerable<Entity> IQueryResult.Result { get => Result; set => Result = value as IEnumerable<TEntity>; }
        IEnumerable<object> IResult.Result { get => Result; set => Result = value as IEnumerable<TEntity>; }
    }
}

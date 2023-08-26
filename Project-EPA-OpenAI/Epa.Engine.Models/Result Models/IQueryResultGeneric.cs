namespace EPA.Engine.Models.Result_Models
{
    public interface IQueryResult<TEntity> : IQueryResult where TEntity : Entity
    {
        new public IEnumerable<TEntity> Result { get; set; }
    }
}

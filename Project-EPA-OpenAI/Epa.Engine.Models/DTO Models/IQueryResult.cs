namespace Epa.Engine.Models.DTO_Models
{
    public interface IQueryResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public IEnumerable<Entity> Result { get; set; }
    }
}

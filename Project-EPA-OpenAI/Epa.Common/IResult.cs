namespace EPA.Common
{
    public interface IResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public IEnumerable<object> Result { get; set; }
    }
}

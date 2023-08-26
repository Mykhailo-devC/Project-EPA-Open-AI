namespace EPA.OpenAI.Models.DTO_Models
{
    public class UserAnswerDTO
    {
        public Guid SessionId { get; set; }
        public string UserName { get; set; }
        public string Answer { get; set; }
    }
}

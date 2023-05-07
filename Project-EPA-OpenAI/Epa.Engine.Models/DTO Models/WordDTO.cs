using System.ComponentModel.DataAnnotations;

namespace Epa.Engine.Models.DTO_Models
{
    public class WordDTO
    {
        [Required]
        [MaxLength(50)]
        public string Value { get; set; }
        public int WordList_Id { get; set; }
    }
}

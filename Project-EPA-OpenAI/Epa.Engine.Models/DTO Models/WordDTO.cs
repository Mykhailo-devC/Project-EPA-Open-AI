using System.ComponentModel.DataAnnotations;

namespace EPA.Engine.Models.DTO_Models
{
    public class WordDTO : DtoEntity
    {
        [Required]
        [MaxLength(50)]
        public string Value { get; set; }
        public int WordList_Id { get; set; } = default;
    }
}

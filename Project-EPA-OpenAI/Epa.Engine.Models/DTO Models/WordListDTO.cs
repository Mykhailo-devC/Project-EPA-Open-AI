using System.ComponentModel.DataAnnotations;

namespace Epa.Engine.Models.DTO_Models
{
    public class WordListDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

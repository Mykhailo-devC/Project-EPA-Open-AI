﻿using System.ComponentModel.DataAnnotations;

namespace EPA.Engine.Models.DTO_Models
{
    public class WordListDTO : DtoEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public List<string> Words { get; set; } = new();
    }
}

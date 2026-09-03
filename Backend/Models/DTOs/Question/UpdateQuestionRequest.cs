using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Question
{
    public class UpdateQuestionRequest
    {
        [Required]

        public int TopicId { get; set; }
        [Required]
        public string Content { get; set; } = null!;
        public string? Difficulty { get; set; }
        public int? Points { get; set; }
    }
}

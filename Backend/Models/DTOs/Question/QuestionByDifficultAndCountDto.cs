using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Question
{
    public class QuestionByDifficultAndCountDto
    {
        [Required]
       public string Difficulty { get; set; }
        public int Count { get; set; } = 10;
    }
}

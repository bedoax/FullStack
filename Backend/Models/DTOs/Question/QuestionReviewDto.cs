using Models.DTOs.Option;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Question
{
    public class QuestionReviewDto
    {
        public int QuestionId { get; set; }

        public string Content { get; set; }

        public string SelectedAnswer { get; set; }

        public string CorrectAnswer { get; set; }

        public bool IsCorrect { get; set; }
        public int Point { get; set; }
        public List<OptionReviewDto> Options { get; set; }

    }
}

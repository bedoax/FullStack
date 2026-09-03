using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.AttemptAnswer
{
    public class SubmitAttemptDto
    {
        public int AttemptId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<AnswerDto> Answers { get; set; }
    }
    public class AnswerDto
    { 
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.QuizQuestionDto
{
    public class CreateQuizQuestionDto
    {
        public int QuizId { get; set; }

        public int QuestionId { get; set; }
    }
}

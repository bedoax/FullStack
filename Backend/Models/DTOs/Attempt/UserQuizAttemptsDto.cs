using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Attempt
{
    public class UserQuizAttemptsDto
    {
        public int QuizId { get; set; }
        public int AttemptsCount { get; set; }
    }
}

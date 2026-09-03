using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Attempt
{
    public class AttemptCreatedDto
    {
        public int AttemptId { get; set; }
        public int QuizId { get; set; }
        public DateTime? StartedAt {  get; set; }
        public DateTime EndsAt { get; set; }
    }
}

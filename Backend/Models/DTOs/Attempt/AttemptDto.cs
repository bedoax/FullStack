using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Attempt
{
   public  class AttemptDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int QuizId { get; set; }

        public int AttemptNumber { get; set; }

        public decimal? Score { get; set; }

        public decimal? Percentage { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public bool? Passed { get; set; }
    }
}

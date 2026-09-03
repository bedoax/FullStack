using Models.DTOs.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Attempt
{
    public class AttemptReviewDto
    {
        public int AttemptId { get; set; }

        public decimal Score { get; set; }

        public decimal Percentage { get; set; }

        public List<QuestionReviewDto> Questions { get; set; }
    }
}

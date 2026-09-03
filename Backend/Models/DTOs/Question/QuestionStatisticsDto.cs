using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Question
{
    public class QuestionStatisticsDto
    {
        public int QuestionId { get; set; }
        public int TimesAnswered { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal SuccessRate { get; set; }
        public int TeacherId { get; set; }
    }
}


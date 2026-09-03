using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Quiz
{
    public class QuizStatisticsDto
    {
        public int TotalStudents { get; set; }
        public int PassedStudents { get; set; }
        public int FailedStudents { get; set; }
        public decimal AverageScore { get; set; }
    }
}

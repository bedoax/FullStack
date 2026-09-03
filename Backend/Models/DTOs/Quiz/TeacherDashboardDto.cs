using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Quiz
{
    public class TeacherDashboardDto
    {
        public int Students { get; set; }
        public int Quizzes { get; set; }
        public int Questions { get; set; }
        public decimal AveragePassRate { get; set; }
    }
}

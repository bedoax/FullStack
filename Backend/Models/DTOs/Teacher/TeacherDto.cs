using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Teacher
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Quizzes { get; set; }
        public int Questions { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

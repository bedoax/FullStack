using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.StudentProfile
{
    public class StudentDashboardDto
    {
       public decimal  SkillScore { get; set; }
       public string CurrentLevel { get; set; }
                 
    public int Attempts { get; set; } 
                 
    public int Passed {  get; set; } 
                 
    public IEnumerable<string> WeakTopics {  get; set; }
    }
}

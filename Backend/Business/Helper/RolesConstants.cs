using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helper
{
    public static class RolesConstants
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string Student = "Student";
        public const string AdminOrTeacher = Admin + "," + Teacher;
        public const string AdminOrTeacherOrStudent = Admin + "," + Teacher + "," + Student;
    }
}

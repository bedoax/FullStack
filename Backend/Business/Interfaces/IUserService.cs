using Models.DTOs.Pagination;
using Models.DTOs.Student;
using Models.DTOs.StudentTopicPerformance;
using Models.DTOs.Teacher;
using Models.DTOs.User;


namespace Business.Interfaces
{
    public interface IUserService
    {
        /*
         Create User
        Update User
        Soft Delete User
        Get User By Id
        Get User By Email
        Get All Users
        Assign Role To User

         */

        Task CreateUser(UserCreateDto dto);

        Task UpdateUser(UserUpdateDto dto);

        Task DeleteUser(int userId);

        Task<UserDto> GetUserById(int userId);

        Task<UserDto> GetUserByEmail(string email);

        Task<IEnumerable<UserDto>> GetAllUsers(int pageNumber, int pageSize);
        // assign it by service when create it or AssignRole
        Task AssignRole(int userId, int roleId);
        Task<IEnumerable<TeacherDto>> GetTeachersAsync();
        Task<PaginatedResult<StudentDto>> GetStudentsAsync(int page, int size);
        Task ActiveUser(int userId);
    }

    public interface IStudentTopicPerformanceService
    {
        /*
 Track Correct Answers
  Track Wrong Answers
  Calculate Success Rate
  Update Performance History

 */
        Task UpdateAfterAttempt(
            int userId,
            Dictionary<int, (int Correct, int Wrong)> topicStats);

        Task<IEnumerable<StudentTopicPerformanceDto>>
            GetPerformanceByUser(int userId);

        Task<StudentTopicPerformanceDto>
            GetPerformanceByTopic(int userId, int topicId);
        Task<IEnumerable<WeakTopicDto>> WeakTopicsOfStudent(int userId);
        Task<List<LeaderboardDto>> GetLeaderboardByTopicId(int topicId);

        
    }
    public interface IDashboardStudentService
    {
        /*
         // we need to (Reporting & Analytics) or dashboard for student
        View Quiz Results
        View Attempt History
        View Topic Performance
        View Overall Performance
        View Current Level
        View Skill Score

 */
    }
    public interface IDashboardTeacherService
    {
        /*
        //we need in teacher dashboard  for his quiz
        Teacher Dashboard
        View Students
        View Student Progress
        View Topic Statistics
        View Quiz Statistics
        View Pass/Fail Rates
         */
    }
    public interface IDashboardAdminService
    {
        /*
         // we need in admin dashboard for all 
         Manage Users
         Manage Roles
         Manage Topics
         Manage Quizzes
         Soft Delete Users
         System Monitoring
         */
    }


}

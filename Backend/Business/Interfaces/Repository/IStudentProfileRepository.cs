using Models.DTOs.StudentProfile;
using Models.Entities;

namespace Business.Interfaces.Repository
{
    public interface IStudentProfileRepository
    {
        Task<StudentProfile>
            GetByUserId(
                int userId);

        Task AddStudentProfile(
            CreateStudentProfileDto profile);

        Task UpdateStudentProfile(
            UpdateStudentProfileDto profile);
        Task<DateTime?> GetLastAssessmentDate(int userId);
        Task<int?> GetTotalAttempts(int userId);
        Task<StudentDashboardDto> Dashboard(int userId,CancellationToken ct);
        Task UpdateStudentProfileAfterSubmition(UpdateStudentProfileDto dto);
        //Task UpdateSkillScore(int userId,decimal skillScore);

        //Task UpdateCurrentLevel(int userId,string currentLevel);
        Task<decimal> GetOverallScore(int userId);
        Task<string> GetCurrentLevel(int userId);
        Task<decimal?> GetSkillScore(int userId);
        Task DeleteStudentProfile(
            int userId);
    }
}

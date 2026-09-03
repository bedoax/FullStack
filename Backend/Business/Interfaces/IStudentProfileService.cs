using Models.DTOs.StudentProfile;
using Models.Entities;


namespace Business.Interfaces
{
    // those can put in Attempt Interface to update and create 
    public interface IStudentProfileService
    {
        /*
         Create Student Profile automatically for students
         Update Student Profile
         Get Student Profile
         Get Current Level
         Get Skill Score
         Get Total Attempts
         Get Last Assessment Date
        View User Profile
         */


        Task<StudentProfile> GetProfile(int userId);

        Task<string> GetCurrentLevel(int userId);
        Task<decimal?> GetSkillScore(int userId);
        Task<DateTime?> GetLastAssessmentDate(int userId);
        Task<int?> GetTotalAttempts(int userId);
        Task UpdateProfileAfterSubmission(int userId, decimal percentage);
        Task<decimal> GetOverallScore(int userId);
        Task<StudentDashboardDto> Dashboard(int userId,CancellationToken ct);
        //Task UpdateProfileAfterSubmission(UpdateStudentProfileDto updateStudentProfileDto);
        // Get Total Attempts
        // Get Last Assessment Date
    }


}

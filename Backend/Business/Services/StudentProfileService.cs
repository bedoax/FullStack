using Business.Interfaces;
using Business.Interfaces.Repository;
using Models.DTOs.StudentProfile;
using Models.Entities;

namespace Business.Services
{
    public class StudentProfileService : IStudentProfileService
    {
        private IUnitOfWork _unitOfWork;
        public StudentProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<string> GetCurrentLevel(int userId)
        {
            return _unitOfWork.StudentProfiles.GetCurrentLevel(userId);
        }
        public Task<decimal?> GetSkillScore(int userId)
        {
            return _unitOfWork.StudentProfiles.GetSkillScore(userId);
        }

        public Task<DateTime?> GetLastAssessmentDate(int userId)
        {
            return _unitOfWork.StudentProfiles.GetLastAssessmentDate(userId);
        }



        public Task<int?> GetTotalAttempts(int userId)
        {
            return _unitOfWork.StudentProfiles.GetTotalAttempts(userId);
        }

/*        public async Task UpdateProfileAfterSubmission(UpdateStudentProfileDto updateStudentProfileDto)
        {

            await _unitOfWork.StudentProfiles.UpdateStudentProfile(updateStudentProfileDto);
        }*/
        public Task<StudentProfile> GetProfile(int userId)
        {
            return _unitOfWork.StudentProfiles.GetByUserId(userId);
        }
        public async Task UpdateProfileAfterSubmission( int userId,decimal percentage)
        {
            var profile = await _unitOfWork.StudentProfiles.GetByUserId(userId);

            if (profile == null)
                throw new KeyNotFoundException();

            profile.TotalAttempts++;
            profile.LastAssessmentDate = DateTime.UtcNow;

            RecalculateSkillScore(profile, percentage);

            RecalculateLevel(profile);
        }
       public  Task<StudentDashboardDto> Dashboard(int userId,CancellationToken ct)
        {
            return _unitOfWork.StudentProfiles.Dashboard(userId,ct);
        }

        private void RecalculateSkillScore(StudentProfile student, decimal percentage)
        {
            student.SkillScore =
                student.SkillScore == null
                    ? percentage
                    : student.SkillScore.Value * 0.8m + percentage * 0.2m;
        }

        private void RecalculateLevel(StudentProfile student)
        {
            if (student.SkillScore >= 80)
                student.CurrentLevel = "Hard";
            else if (student.SkillScore >= 50)
                student.CurrentLevel = "Medium";
            else
                student.CurrentLevel = "Easy";
        }
        public Task<decimal> GetOverallScore(int userId)
        {
            return _unitOfWork.StudentProfiles.GetOverallScore(userId);
        }

    }
}

using Business.Helper;
using Business.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Models.DTOs.StudentProfile;
using Models.Entities;
namespace Data.Repositories
{
    public class StudentProfileRepository : IStudentProfileRepository
    {
        private  AppDbContext _context;
        public StudentProfileRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddStudentProfile(CreateStudentProfileDto dto)
        {
            var exists = await _context.StudentProfiles
                .FirstOrDefaultAsync(p => p.UserId == dto.UserId);
            
            if (exists != null)
                throw new Exception("Student profile already exists");

            var profile = new StudentProfile
            {
                UserId = dto.UserId,
                CurrentLevel = dto.CurrentLevel,
                SkillScore = dto.SkillScore,
                TotalAttempts = dto.TotalAttempts,
                LastAssessmentDate = dto.LastAssessmentDate
            };

            _context.StudentProfiles.Add(profile);
        }

        public Task DeleteStudentProfile(int userId)
        {
            // only use it when we want to delte user and it will delete also with it (Cascade Delete)
            throw new NotImplementedException();
        }

        public async Task<StudentProfile> GetByUserId(int userId)
        {
            return await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        // will cost more quires to the database if we update each property separately, so we will not implement this method for now
        /*        public Task UpdateCurrentLevel(int userId, string currentLevel)
        {
            throw new NotImplementedException();
        }
        public Task UpdateSkillScore(int userId, decimal skillScore)
        {
            throw new NotImplementedException();
        }*/

        public async Task UpdateStudentProfile(UpdateStudentProfileDto dto)
        {
            var profile = await _context.StudentProfiles
                              .FirstOrDefaultAsync(p => p.UserId == dto.UserId);

            if (profile == null)
                throw new KeyNotFoundException("Profile not found");

            if (dto.CurrentLevel != null)
                profile.CurrentLevel = dto.CurrentLevel;

            if (dto.SkillScore.HasValue)
                profile.SkillScore = dto.SkillScore;

            if (dto.TotalAttempts.HasValue)
                profile.TotalAttempts = dto.TotalAttempts;

            if (dto.LastAssessmentDate.HasValue)
                profile.LastAssessmentDate = dto.LastAssessmentDate;
        }
        public async Task UpdateStudentProfileAfterSubmition(UpdateStudentProfileDto dto)
        {
           var rowsAffected =  await _context.StudentProfiles.Where(p => p.UserId == dto.UserId).ExecuteUpdateAsync(s => 
                s.SetProperty(p => p.CurrentLevel, dto.CurrentLevel)
                .SetProperty(p => p.SkillScore, dto.SkillScore)
                .SetProperty(p => p.TotalAttempts, dto.TotalAttempts)
                .SetProperty(p => p.LastAssessmentDate, dto.LastAssessmentDate));

            if (rowsAffected == 0)
                throw new KeyNotFoundException("Profile not found");
        }

        public async Task<DateTime?> GetLastAssessmentDate(int userId)
        {
            return await _context.StudentProfiles
              .Where(x => x.UserId == userId)
             .Select(x => x.LastAssessmentDate)    
              .FirstOrDefaultAsync();
        }
        public async Task<int?> GetTotalAttempts(int userId)
        {
            return await _context.StudentProfiles
              .Where(x => x.UserId == userId)
             .Select(x => x.TotalAttempts)
              .FirstOrDefaultAsync();
        }
        public async Task<string?> GetCurrentLevel(int userId)
        {
            return await _context.StudentProfiles
              .Where(x => x.UserId == userId)
             .Select(x => x.CurrentLevel)
              .FirstOrDefaultAsync();
        }
        public async Task<decimal?> GetSkillScore(int userId)
        {
            return await _context.StudentProfiles.Where(x => x.UserId == userId).Select(x => x.SkillScore).FirstOrDefaultAsync();
        }
        public async Task<decimal> GetOverallScore(int userId)
        {
            return await _context.Attempts
                .Where(x =>
                    x.UserId == userId &&
                    x.SubmittedAt != null)
                .AverageAsync(x => (decimal?)x.Percentage) ?? 0;
        }
        public  async Task<StudentDashboardDto> Dashboard(int userId,CancellationToken ct)
        {
                // Current implementation uses EF Core projection.
                // If dashboard calculations become expensive at scale,
                // consider moving aggregation logic to a stored procedure.
                    var dashboard = await _context.StudentProfiles
                             .AsNoTracking()
                             .Where(sp => sp.UserId == userId)
                             .Select(sp => new StudentDashboardDto
                             {
                                 SkillScore = sp.SkillScore ?? 0,

                                 CurrentLevel = sp.CurrentLevel,

                                 Attempts = _context.Attempts
                                     .Count(a => a.UserId == userId),

                                 Passed = _context.Attempts
                                     .Count(a =>
                                         a.UserId == userId &&
                                         a.Passed == true),

                                 WeakTopics = _context.StudentTopicPerformances
                                     .Where(stp =>
                                         stp.UserId == userId &&
                                         stp.SuccessRate < 60)
                                     .Select(stp => stp.Topic.Name)
                                     .ToList()
                             })
                             .FirstOrDefaultAsync(ct);
            if(dashboard == null)
            {
                return new StudentDashboardDto
                {
                    SkillScore = 0,
                    Attempts = 0,
                    Passed = 0,
                    WeakTopics = new List<string>(),
                    CurrentLevel = LevelConstans.Easy
                };
            }
            return dashboard;

        }
    }
}



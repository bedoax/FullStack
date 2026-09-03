using Models.DTOs.Attempt;
using Models.DTOs.AttemptAnswer;


namespace Business.Interfaces
{
    public interface IAttemptService
    {
        /*

        Create Attempt

        Check Max Attempts
        
        Submit Attempt
        
        Validate Answers
        
        Calculate Score
        
        Calculate Percentage
        
        Determine Pass/Fail
        
        Save Attempt Answers
        
        Update Attempt Result
        
        Update StudentProfile
        
        Update TopicPerformance
        
        Detect Weak Topics
        
        Detect Strong Topics
        
        Get Attempt Details
        
        Get User Attempts
        
        Get Attempts Count Per Quiz
        
        // we can make  interface hold those 
        Submit Attempt
        Validate Submitted Answers
        Calculate Score
        Calculate Percentage
        Determine Pass/Fail
        those prevoues should update entity the student profile and student topic 
        Save Attempt Answers
        Save Final Result

         */

        Task<AttemptCreatedDto> CreateAttempt(CreateAttemptDto dto);

        
            Task SubmitAttempt(SubmitAttemptDto dto);

        Task<IEnumerable<AttemptListDto>> GetUserAttempts(int userId,CancellationToken ct);
        Task<AttemptReviewDto> ReviewMyAttempt(int userId, int attemptId);
        Task<AttemptWithQuizDto> GetAttemptWithQuizDetails(int attemptId);

        /*
         which one is she 
        //Average Percentage ? 
        // Average Score ? 
        // StudentProfile.SkillScore ? 
         */
        Task<decimal> GetUserOverallScore(int userId);
    }


}

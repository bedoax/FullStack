namespace Models.DTOs.StudentProfile
{
    public class UpdateStudentProfileDto
    {
        public int UserId { get; set; }
        public string? CurrentLevel { get; set; }
        public decimal? SkillScore { get; set; } 
        public int? TotalAttempts { get; set; } 
        public DateTime? LastAssessmentDate { get; set; }
    }
}

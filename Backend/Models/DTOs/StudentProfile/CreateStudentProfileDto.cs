namespace Models.DTOs.StudentProfile
{
    public class CreateStudentProfileDto
    {
        public int UserId { get; set; }
        public string? CurrentLevel { get; set; } = null;
        public decimal? SkillScore { get; set; } = null;
        public int? TotalAttempts { get; set; } = null;
        public DateTime? LastAssessmentDate { get; set; } = null;
    }
}

using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class StudentProfile
{
    public int UserId { get; set; }

    public string? CurrentLevel { get; set; }

    public decimal? SkillScore { get; set; }

    public int? TotalAttempts { get; set; }

    public DateTime? LastAssessmentDate { get; set; }

    public virtual User User { get; set; } = null!;
}

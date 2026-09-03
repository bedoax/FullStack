using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Attempt
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int QuizId { get; set; }

    public int AttemptNumber { get; set; }

    public decimal? Score { get; set; }

    public decimal? Percentage { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? SubmittedAt { get; set; }
    public DateTime EndsAt { get; set; }
    public bool IsAutoSubmitted { get; set; }

    public bool? Passed { get; set; }

    public virtual ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}


using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class StudentTopicPerformance
{
    public int UserId { get; set; }

    public int TopicId { get; set; }

    public int? CorrectAnswers { get; set; }

    public int? WrongAnswers { get; set; }

    public decimal? SuccessRate { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Topic Topic { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

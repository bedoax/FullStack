using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class AttemptAnswer
{
    public int Id { get; set; }

    public int AttemptId { get; set; }

    public int QuestionId { get; set; }

    public int? SelectedOptionId { get; set; }

    public bool? IsCorrect { get; set; }

    public int? EarnedPoints { get; set; }

    public virtual Attempt Attempt { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;

    public virtual Option? SelectedOption { get; set; }
}

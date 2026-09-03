using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Question
{
    public int Id { get; set; }

    public int TopicId { get; set; }

    public string Content { get; set; } = null!;

    public string? Difficulty { get; set; }

    public int? Points { get; set; }
    public int CreatedByTeacherId { get; set; }

    public User CreatedByTeacher { get; set; } = null!;

    public virtual ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();

    public virtual Topic Topic { get; set; } = null!;

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}

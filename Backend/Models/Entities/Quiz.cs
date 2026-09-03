

namespace Models.Entities;

public partial class Quiz
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? MaxAttempts { get; set; }

    public decimal? PassPercentage { get; set; }
    public bool IsPublished { get; set; } = false;

    public DateTime? CreatedAt { get; set; }
    public int CreatedByTeacherId { get; set; }

    public User CreatedByTeacher { get; set; } = null!;

    public int DurationMinutes { get; set; }
    // nullable for when make it draft
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableTo { get; set; }

    public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}

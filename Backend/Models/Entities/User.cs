using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public int RoleId { get; set; }
    public bool IsDeleted { get; set; } = false;

    public DateTime? CreatedAt { get; set; }
    public string? GoogleId { get; set; }

    public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();

    public virtual Role Role { get; set; } = null!;

    public virtual StudentProfile? StudentProfile { get; set; }

    public ICollection<Question> CreatedQuestions { get; set; }
    
    public ICollection<Quiz> CreatedQuizzes { get; set; }
    public virtual ICollection<StudentTopicPerformance> StudentTopicPerformances { get; set; } = new List<StudentTopicPerformance>();
    public ICollection<RefreshToken> RefreshTokens{get; set;}
    public virtual ICollection<PasswordResetOtp> PasswordResetOtps { get; set; } = new List<PasswordResetOtp>();
}

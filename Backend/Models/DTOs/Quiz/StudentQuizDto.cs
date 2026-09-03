using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Quiz
{
    public class StudentQuizDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public int MaxAttempts { get; set; }

        public decimal PassPercentage { get; set; }

        // Student Data
        public int AttemptsUsed { get; set; }

        public bool HasActiveAttempt { get; set; }

        public DateTime? EndsAt { get; set; }

        public bool CanStart { get; set; }

        public bool IsAvailable { get; set; }
        public bool? Passed { get; set; }
        public int ? ActiveAttemptId { get; set; }
        public int ? ActiveQuizId { get; set; }
    }
}

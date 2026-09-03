using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Topic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<StudentTopicPerformance> StudentTopicPerformances { get; set; } = new List<StudentTopicPerformance>();
}

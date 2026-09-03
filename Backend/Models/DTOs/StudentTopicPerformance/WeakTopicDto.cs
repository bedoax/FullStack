using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.StudentTopicPerformance
{
    public class WeakTopicDto
    {
        public int TopicId { get; set; }

        public string TopicName { get; set; }

        public decimal SuccessRate { get; set; }

        public int TotalQuestionsSolved { get; set; }
    }
}

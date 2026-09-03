using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Option
{
    public class OptionReviewDto
    {
        public int OptionId { get; set; }

        public string Content { get; set; }

        public bool IsCorrect { get; set; }

        public bool IsSelectedByStudent { get; set; }
    }
}

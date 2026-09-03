using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Pagination
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}

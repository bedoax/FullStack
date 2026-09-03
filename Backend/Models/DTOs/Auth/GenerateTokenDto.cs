using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Auth
{
    public  class GenerateTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt {  get; set; }
    }
}

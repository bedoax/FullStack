using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Role
{
    public  class RoleUpdateRquest
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}

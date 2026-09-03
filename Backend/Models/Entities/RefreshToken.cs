using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Token { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }
        /*
         Refresh Token Rotation with ReplacedByToken variable
استقبال A
↓
التأكد أنه صالح
↓
إنشاء B
↓
Revoke A
↓
إرجاع B

        انما لو في تسريب ، علي طول بنصفر السلسلة وبيعيد التسجيل من جديد 
        
         */
    }
}

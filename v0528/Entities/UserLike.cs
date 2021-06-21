using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace v0528.Entities
{
    [Table("Likes")]
    public class UserLike
    {
        public virtual AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public virtual AppUser LikedUser { get; set; }
        public int LikedUserId { get; set; }

    }
}

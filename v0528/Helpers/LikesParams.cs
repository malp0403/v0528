using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace v0528.Helpers
{
    public class LikesParams:PaginationParams
    {
        public string predicate { get; set; }
        public int UserId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Entities;

namespace v0528.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Entities;
using v0528.Helpers;
using v0528.Models;

namespace v0528.Interface
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PageList<MemberModel>> GetMembersAsync(UserParams userParams);
        Task<MemberModel> GetMemberAsync(string username);
    }
}

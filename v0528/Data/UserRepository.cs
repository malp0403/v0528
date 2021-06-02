using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Entities;
using v0528.Interface;
using v0528.Models;

namespace v0528.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(x=>x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(x=>x.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<MemberModel> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberModel>> GetMembersAsync()
        {
            return await _context.Users
                .ProjectTo<MemberModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

   
    }
}

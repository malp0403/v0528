using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Entities;
using v0528.Extensions;
using v0528.Helpers;
using v0528.Interface;
using v0528.Models;

namespace v0528.Data
{
    public class LikesRepostiory : ILikesRespository
    {
        private readonly DataContext _context;

        public LikesRepostiory(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
            //throw new NotImplementedException();
        }

        public async Task<PageList<LikeModel>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.predicate == "liked")
            {
                likes = likes.Where(x => x.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }
            if(likesParams.predicate == "likedBy")
            {
                likes = likes.Where(x => x.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }



            var likeUsers = users.Select(user => new LikeModel
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City= user.City,
                Id = user.Id
            });

            return await PageList<LikeModel>.CreateAsync(likeUsers, likesParams.PageNumber, likesParams.PageSize);

        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}

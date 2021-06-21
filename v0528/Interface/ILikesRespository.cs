using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Entities;
using v0528.Helpers;
using v0528.Models;

namespace v0528.Interface
{
    public interface ILikesRespository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PageList<LikeModel>> GetUserLikes(LikesParams likesParams);
    }
}

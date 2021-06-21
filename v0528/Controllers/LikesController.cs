using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using v0528.Entities;
using v0528.Extensions;
using v0528.Helpers;
using v0528.Interface;
using v0528.Models;

namespace v0528.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRespository _likesRepository;

        public LikesController(IUserRepository userRepository, ILikesRespository likesRepository)
        {
            _userRepository = userRepository;
            _likesRepository = likesRepository;
        }

        [HttpPost]
        [Route("[action]/{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("cant like your self");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("your already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Failed to like user");

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<LikeModel>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}
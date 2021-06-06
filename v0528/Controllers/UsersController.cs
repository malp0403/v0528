using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using v0528.Data;
using v0528.Entities;
using v0528.Extensions;
using v0528.Interface;
using v0528.Models;

namespace v0528.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<MemberModel>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{username}",Name ="GetUser")]
        public async Task<ActionResult<MemberModel>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
            //var user = _userRepository.GetUserByUsernameAsync(username);
            //var userModel = _mapper.Map<MemberModel>(user);
            //return userModel;
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdateUser([FromBody]MemberUpdateModel memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("failed to update user");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<PhotoModel>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = user.Photos.Count == 0,
                AppUser = user
            };

            user.Photos.Add(photo);
            if(await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser",new { username = user.UserName}, _mapper.Map<PhotoModel>(photo));
            }
            return BadRequest("Photo save unsuccessfully");

        }

        [HttpPost]
        [Route("[action]/{photoId}")]

        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("it is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain != null)
            {
                currentMain.IsMain = false;
            }
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("fail to set main photo");

        }


        [HttpPost]
        [Route("[action]/{photoId}")]

        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("you cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }

            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("failed to delete the photo");
        }
    }
}
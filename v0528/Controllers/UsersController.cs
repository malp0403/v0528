using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using v0528.Data;
using v0528.Entities;
using v0528.Interface;
using v0528.Models;

namespace v0528.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
        [Route("[action]/{username}")]
        public async Task<ActionResult<MemberModel>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
            //var user = _userRepository.GetUserByUsernameAsync(username);
            //var userModel = _mapper.Map<MemberModel>(user);
            //return userModel;
        }
    }
}
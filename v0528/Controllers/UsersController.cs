﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using v0528.Data;
using v0528.Entities;

namespace v0528.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public UsersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _dataContext.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int Id)
        {
            return await _dataContext.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
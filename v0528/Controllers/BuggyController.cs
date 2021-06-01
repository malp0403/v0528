using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using v0528.Data;
using v0528.Entities;

namespace v0528.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return Ok(thing);
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString();
            return thingToReturn;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<string> GetBadrequest()
        {
            return BadRequest("it is bad Request");
        }
    }
}
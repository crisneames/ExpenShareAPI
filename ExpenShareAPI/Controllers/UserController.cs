using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenShareAPI.Repositories;
using ExpenShareAPI.Models;

namespace ExpenShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userRepository.GetAllUsers());
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public IActionResult Post(User user)
        {
            _userRepository.Add(user);
            return CreatedAtAction("Get", new { id = user.Id }, user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _userRepository.Update(user);
            return Ok(user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userRepository.Delete(id);
            return NoContent();
        }
    }
}

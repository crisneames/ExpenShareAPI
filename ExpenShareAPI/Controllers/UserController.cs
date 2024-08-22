using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenShareAPI.Repositories;
using ExpenShareAPI.Models;
using ExpenShareAPI.Utils;

namespace ExpenShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordService _passwordService;
        private readonly UserService _userService;

        public UserController(IUserRepository userRepository, PasswordService passwordService, UserService userService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _userService = userService;

        }

        // GET: api/user/{userName}
        [HttpGet("{userName}")]
        public IActionResult GetByUserName(string userName)
        {
            var user = _userRepository.GetByUserName(userName);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            return Ok(user);
        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            var hashedPassword = _passwordService.HashPassword(request.Password);

            // Save the hashed password along with other details to the database
            var newUser = new User
            {
                UserName = request.UserName,
                PasswordHash = hashedPassword,
                Email = request.Email,
                FullName = request.FullName,
                CreatedAt = DateTime.Now
            };

            _userRepository.Add(newUser);
            return Ok();
        }

        // GET: api/User
        // [HttpGet]
        // public IActionResult Get()
        // {
        //    return Ok(_userRepository.GetAllUsers());
        // }

        // GET: api/User/5
        // [HttpGet("{id}")]
        // public IActionResult Get(int id)
        // {
        //    var user = _userRepository.GetById(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        // }

        // POST: api/User
        // [HttpPost]
        // public IActionResult Post(User user)
        //{
        //    _userRepository.Add(user);
        //    return CreatedAtAction("Get", new { id = user.Id }, user);
        //}

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

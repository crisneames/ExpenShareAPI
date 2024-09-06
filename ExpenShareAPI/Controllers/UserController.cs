using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenShareAPI.Repositories;
using ExpenShareAPI.Models;
using ExpenShareAPI.Utils;
using System.Runtime.CompilerServices;

namespace ExpenShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly PasswordService _passwordService;
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public UserController(IUserRepository userRepository, ITokenRepository tokenRepository, PasswordService passwordService, UserService userService, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _passwordService = passwordService;
            _userService = userService;
            _tokenService = tokenService;
        }

        // GET: api/user/{userName}
        [HttpGet("{UserName}")]
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

        // POST: api/user/authenticate
        [HttpPost("authenticate")]
        // [ValidateAntiForgeryToken] // Ensure CSRF token validation
        public IActionResult Authenticate([FromBody] AuthenticateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "Invalid username or password" });
            }

            // Retrieve the user by username
            var user = _userRepository.GetByUserName(request.UserName);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            // Verify the provided password against the stored hashed password
            bool isPasswordValid = _passwordService.VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }
            return Ok();

            // Generate a JWT token for the user

            var token = _tokenService.GenerateToken(user);

            var tokenEntity = new Token
            {
                UserId = user.Id,
                RefreshToken = token,
                ExpiresAt = DateTime.Now.AddMinutes(30),
                CreatedAt = DateTime.Now,
                RevokedAt = null
            };

            try
            {
                _tokenRepository.Add(tokenEntity);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine("Failed to store token: " + ex.ToString());
                return StatusCode(500, new { Message = "Internal server error while storing token." });
            }

            // If authentication is successful, you might return some user data
            // In a real-world scenario, you could also generate a JWT token and return it
            return Ok(new 
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FullName,
                Token = token,
            });
        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenShareAPI.Repositories;
using ExpenShareAPI.Models;
using ExpenShareAPI.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace ExpenShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordService _passwordService;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, PasswordService passwordService, UserService userService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _userService = userService;
            _configuration = configuration;
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

            if (newUser == null) 
            {
                return BadRequest(new { message = "User creation failed" });
            }

            return Ok(new { message = "User created successfully"});
        }

        [EnableCors("AllowAllOrigins")]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLogin login)
        {
            Console.WriteLine("API called with UserName: " + login.UserName); //log the request

            var user = _userRepository.GetByUserName(login.UserName);  // Compare the userName field

            // Verify the provided password against the stored hashed password
            bool isPasswordValid = _passwordService.VerifyPassword(login.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            var token = GenerateJwtToken(user);
            Console.WriteLine("Generated user " + user.UserName);
            Console.WriteLine("Generated Token: " + token);  // Log the token
            Console.WriteLine($"Generated token for user ID: {user.Id}");

            return Ok(new { id = user.Id,
                userName = user.UserName,
                fullName = user.FullName,
                email = user.Email,
                token });

        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("test")]
        public IActionResult TestJson([FromBody] Dictionary<string, string> data)
        {
            return Ok(new { message = "Received JSON", data });
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


// Mocked check for email/password (replace with database check) from CN ChatGPT
/*
 * if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))  // Compare the hashedPassword field
            {
                return Unauthorized(new { message = "Invalid userName or password" });
            }

            return Unauthorized();
*/
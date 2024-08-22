using ExpenShareAPI.Models;
using ExpenShareAPI.Repositories;
using System.Text.RegularExpressions;

namespace ExpenShareAPI.Utils;

public class UserService
{
    private readonly PasswordService _passwordService;
    private readonly IUserRepository _userRepository;

    public UserService(PasswordService passwordService, IUserRepository userRepository)
    {
        _passwordService = passwordService;
        _userRepository = userRepository;
    }

    public void CreateUser(CreateUserRequest request)
    {
        // Validate username uniqueness
        if (_userRepository.GetByUserName(request.UserName) != null)
        {
            throw new ArgumentException("Username is already taken.");
        }

        // Validate password
        if (!IsValidPassword(request.Password))
        {
            throw new ArgumentException("Password does not meet the criteria.");
        }

        var hashedPassword = _passwordService.HashPassword(request.Password);

        var user = new User
        {
            UserName = request.UserName,
            PasswordHash = hashedPassword,
            Email = request.Email,
            FullName = request.FullName,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _userRepository.Add(user);
    }

    private bool IsValidPassword(string password)
    {
        // Example criteria: at least 8 characters, including one number and one uppercase letter
        return !string.IsNullOrEmpty(password) &&
               password.Length >= 8 &&
               Regex.IsMatch(password, @"[A-Z]") &&
               Regex.IsMatch(password, @"\d");
    }
}



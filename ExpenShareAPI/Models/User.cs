using System;
namespace ExpenShareAPI.Models
{
	public class User
	{
        public int Id { get; set; } // Primary Key, auto-increment
        public string UserName { get; set; } // Unique username
        public string PasswordHash { get; set; } // Hashed password
        public string? Email { get; set; } // Optional, user's email
        public string? FullName { get; set; } // Optional, full name of the user
        public DateTime? CreatedAt { get; set; } // Timestamp for when the user was created
        public DateTime? UpdatedAt { get; set; } // Timestamp for when the user was last updated
        public DateTime? LastLogin { get; set; } // Optional, timestamp for the last login

    }

	public class CreateUserRequest
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
        public string FullName { get; set; }
	}

    public class AuthenticateRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}


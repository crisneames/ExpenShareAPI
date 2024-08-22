using BCrypt.Net;

namespace ExpenShareAPI.Utils;
public class PasswordService
{
    // Method to hash a password
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Method to verify a password against a hash
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

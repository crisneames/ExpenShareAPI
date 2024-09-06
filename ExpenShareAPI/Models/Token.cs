namespace ExpenShareAPI.Models;

public class Token
{
    public int TokenId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } // Navigation property to the User entity
}


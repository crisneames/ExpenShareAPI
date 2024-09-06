using ExpenShareAPI.Models;

namespace ExpenShareAPI.Repositories
{
    public interface ITokenRepository
    {
        void Add(Token token);
        Token GetTokenByUserId(int userId);
    }
}
using ExpenShareAPI.Models;

namespace ExpenShareAPI.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
    }
}
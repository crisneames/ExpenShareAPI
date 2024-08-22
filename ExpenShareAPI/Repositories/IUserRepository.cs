using ExpenShareAPI.Models;

namespace ExpenShareAPI.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        void Delete(int id);
        User GetByUserName(string userName);
        void Update(User user);
    }
}
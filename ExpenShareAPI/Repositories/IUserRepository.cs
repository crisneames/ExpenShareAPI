using ExpenShareAPI.Models;

namespace ExpenShareAPI.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetById(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);


    }
}
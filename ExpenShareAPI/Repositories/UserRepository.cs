using System;
using ExpenShareAPI.Models;
using ExpenShareAPI.Utils;

namespace ExpenShareAPI.Repositories;


public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration) { }

    public List<User> GetAllUsers()
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT id,
                                            name,
                                             email 
                                   FROM [user]";
                var reader = cmd.ExecuteReader();
                var users = new List<User>();

                while (reader.Read())
                {
                    var user = new User()
                    {
                        Id = DbUtils.GetInt(reader, "id"),
                        Name = DbUtils.GetString(reader, "name"),
                        Email = DbUtils.GetString(reader, "email")
                    };

                    users.Add(user);
                }

                conn.Close();
                return users;
            }
        }
    }
}



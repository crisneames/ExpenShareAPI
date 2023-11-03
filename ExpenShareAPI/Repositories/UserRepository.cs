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

    public User GetById(int id)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT id,
                                            name,
                                             email 
                                    FROM [user]
                                    WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                User user = null;

                if (reader.Read())
                {
                    user = new User()
                    {
                        Id = DbUtils.GetInt(reader, "id"),
                        Name = DbUtils.GetString(reader, "name"),
                        Email = DbUtils.GetString(reader, "email")

                    };
                }
                reader.Close();
                return user;
            }
        }
    }
    public void Add(User user)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                      INSERT INTO [user] (name, email)
                        OUTPUT INSERTED.ID
                        VALUES (@name, @email)";

                DbUtils.AddParameter(cmd, "@name", user.Name);
                DbUtils.AddParameter(cmd, "@email", user.Email);
              

                user.Id = (int)cmd.ExecuteScalar();
            }
        }
    }

    public void Update(User user)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                      UPDATE [user]
                         SET name = @name,
                              email = @email
                         WHERE id = @id";

                DbUtils.AddParameter(cmd, "@id", user.Id);
                DbUtils.AddParameter(cmd, "@name", user.Name);
                DbUtils.AddParameter(cmd, "@email", user.Email);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Delete(int id)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM [user] WHERE id = @id";
                DbUtils.AddParameter(cmd, "@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}



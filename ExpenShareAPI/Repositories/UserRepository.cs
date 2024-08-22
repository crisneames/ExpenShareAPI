using System;
using System.Data.SqlTypes;
using System.Security.Cryptography.Pkcs;
using ExpenShareAPI.Models;
using ExpenShareAPI.Utils;
using Microsoft.Data.SqlClient;

namespace ExpenShareAPI.Repositories;


public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration) { }

    public User GetByUserName(string userName)
    {
        User user = null;
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM [user] WHERE userName = @userName";
                cmd.Parameters.AddWithValue("@userName", userName);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                            FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString(reader.GetOrdinal("FullName")),
                            CreatedAt = DbUtils.GetNullableDateTime(reader, "CreatedAt"),
                            UpdatedAt = DbUtils.GetNullableDateTime(reader, "UpdatedAt"),
                            LastLogin = DbUtils.GetNullableDateTime(reader, "LastLogin")
                        };
                    }
                }
            }
        }
        return user;
    }
    /* public list<user> getallusers()
    {
        using (var conn = connection)
        {
            conn.open();
            using (var cmd = conn.createcommand())
            {
                cmd.commandtext = @"select id,
                                            name,
                                             email 
                                   from [user]";
                var reader = cmd.executereader();
                var users = new list<user>();

                while (reader.read())
                {
                    var user = new user()
                    {
                        id = dbutils.getint(reader, "id"),
                        name = dbutils.getstring(reader, "name"),
                        email = dbutils.getstring(reader, "email")
                    };

                    users.add(user);
                }

                conn.close();
                return users;
            }
        }
    } */

    /* public User GetById(int id)
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
    } */

    public void Add(User user)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand()) 
            {
                cmd.CommandText = @"INSERT INTO [user] (userName, passwordHash, email, fullName, createdAt, updatedAt) 
                                    VALUES (@userName, @passwordHash, @email, @fullName, @createdAt, @updatedAt)";

                DbUtils.AddParameter(cmd, "@userName", user.UserName);
                DbUtils.AddParameter(cmd, "@passwordHash", user.PasswordHash);
                DbUtils.AddParameter(cmd, "@email", user.Email);
                DbUtils.AddParameter(cmd, "@fullName", user.FullName);

                // Handle nullable DateTime values
                DbUtils.AddParameter(cmd, "@createdAt", DbUtils.GetSqlCompatibleDateTime(user.CreatedAt));
                DbUtils.AddParameter(cmd, "@updatedAt", DbUtils.GetSqlCompatibleDateTime(user.UpdatedAt));

                cmd.ExecuteNonQuery();

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
                         SET userName = @userName,
                                email = @email,
                             fullName = @fullName
                         WHERE id = @id";

                DbUtils.AddParameter(cmd, "@id", user.Id);
                DbUtils.AddParameter(cmd, "@name", user.UserName);
                DbUtils.AddParameter(cmd, "@email", user.Email);
                DbUtils.AddParameter(cmd, "@fullName", user.FullName);

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



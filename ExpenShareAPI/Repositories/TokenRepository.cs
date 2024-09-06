using ExpenShareAPI.Models;
using ExpenShareAPI.Utils;
using Microsoft.Data.SqlClient;

namespace ExpenShareAPI.Repositories;

public class TokenRepository : BaseRepository, ITokenRepository
{
    public TokenRepository(IConfiguration configuration) : base(configuration) { }

    public void Add(Token token)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO tokens (accessToken, expiresAt, createdAt, userId) 
                                        VALUES (@accessToken, @expiresAt, @createdAt, @userId)";

                DbUtils.AddParameter(cmd, "@accessToken", token.AccessToken);
                DbUtils.AddParameter(cmd, "@expiresAt", token.ExpiresAt);
                DbUtils.AddParameter(cmd, "@createdAt", token.CreatedAt);
                DbUtils.AddParameter(cmd, "@userId", token.UserId);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public Token GetTokenByUserId(int userId)
    {
        using (var conn = Connection)
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT tokenId,
                                           accessToken,
                                           expiresAt,
                                           createdAt,
                                           userId
                                      FROM tokens
                                     WHERE userId = @userId";

                DbUtils.AddParameter(cmd, "@userId", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Token
                        {
                            TokenId = DbUtils.GetInt(reader, "tokenId"),
                            AccessToken = DbUtils.GetString(reader, "accessToken"),
                            ExpiresAt = DbUtils.GetDateTime(reader, "expiresAt"),
                            CreatedAt = DbUtils.GetDateTime(reader, "createdAt"),
                            UserId = DbUtils.GetInt(reader, "userId")
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}

using System;
using ExpenShareAPI.Models;
using ExpenShareAPI.Utils;

namespace ExpenShareAPI.Repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        public EventRepository(IConfiguration configuration) : base(configuration) { }

        public List<Event> GetEvents()
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM event";

                    var reader = cmd.ExecuteReader();

                    var events = new List<Event>();

                    while (reader.Read())
                    {
                        var gig = new Event()
                        {
                            Id = DbUtils.GetInt(reader, "id"),
                            Name = DbUtils.GetString(reader, "name"),
                            Date = DbUtils.GetDateTime(reader, "date"),
                            Comment = DbUtils.GetString(reader, "comment")
                        };

                        events.Add(gig);
                    }
                    conn.Close();
                    return events;
                }
            }
        }

        public Event GetEventById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM event WHERE id = @id";

                    cmd.Parameters.AddWithValue("id", id);
                    var reader = cmd.ExecuteReader();
                    Event gig = null;

                    while (reader.Read())
                    {
                        gig = new Event()
                        {
                            Id = DbUtils.GetInt(reader, "id"),
                            Name = DbUtils.GetString(reader, "name"),
                            Date = DbUtils.GetDateTime(reader, "date"),
                            Comment = DbUtils.GetString(reader, "comment")
                        };
                    }
                    conn.Close();
                    return gig;
                }
            }
        }

        public void AddNewEvent(Event gig)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO event ([name], [date], comment)
                                        OUTPUT INSERTED.ID
                                        VALUES (@name, @date, @comment)";

                    DbUtils.AddParameter(cmd, "@name", gig.Name);
                    DbUtils.AddParameter(cmd, "@date", gig.Date);
                    DbUtils.AddParameter(cmd, "@comment", gig.Comment);

                    gig.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateEvent(Event gig)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE event
                                           SET name = @name,
	                                           date = @date,
	                                           comment = @comment
                                         WHERE id = @id";

                    DbUtils.AddParameter(cmd, "@id", gig.Id);
                    DbUtils.AddParameter(cmd, "@name", gig.Name);
                    DbUtils.AddParameter(cmd, "date", gig.Date);
                    DbUtils.AddParameter(cmd, "comment", gig.Comment);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteEvent(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM event WHERE id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Event GetEventWithUsers(int EventId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT event.id as EventId
	                                           ,[event].name
	                                           ,[event].date
	                                           ,[event].comment
	                                           ,[user].name as UserName
                                               ,[user].email
                                               ,userEvent.userId as UserId
                                        FROM event
                                        JOIN userEvent on EventId = userEvent.eventId
                                        JOIN [user] on [user].id = userEvent.userId
                                        WHERE event.id = @EventId";

                    cmd.Parameters.AddWithValue("EventId", EventId);

                    var reader = cmd.ExecuteReader();

                    Event gig = null;

                    while (reader.Read())
                    {
                        gig = new Event
                        {
                            Id = DbUtils.GetInt(reader, "EventId"),
                            Name = DbUtils.GetString(reader, "name"),
                            Date = DbUtils.GetDateTime(reader, "date"),
                            Comment = DbUtils.GetString(reader, "comment"),
                            User = new List<User>()
                        };

                        if (DbUtils.IsNotDbNull(reader, "UserId"))
                        {
                            gig.User.Add(new User()
                            {
                                Id = DbUtils.GetInt(reader, "UserId"),
                                Name = DbUtils.GetString(reader, "UserName"),
                                Email = DbUtils.GetString(reader, "email")
                            });
                        }
                    }

                    reader.Close();
                    return gig;
                }
            }
        }
    }
}

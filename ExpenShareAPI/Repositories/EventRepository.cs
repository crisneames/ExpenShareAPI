using System;
using ExpenShareAPI.Models;
using ExpenShareAPI.Utils;
using NuGet.DependencyResolver;

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

        /* public Event GetEventWithUsers(int EventId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT event.id AS EventId
	                                           ,[event].name
	                                           ,[event].date
	                                           ,[event].comment
	                                           ,[user].name AS UserName
                                               ,[user].email
                                               ,[user].id AS UserId
                                        FROM event
                                        JOIN userEvent on EventId = userEvent.eventId
                                        JOIN [user] on [user].id = userEvent.userId
                                        WHERE event.id = @EventId";

                    cmd.Parameters.AddWithValue("@EventId", EventId);

                    var reader = cmd.ExecuteReader();

                    Event gig = new();

                    while (reader.Read())
                    {
                        if(gig.Id == 0)
                        {
                            gig = new Event()
                            {
                                Id = DbUtils.GetInt(reader, "EventId"),
                                Name = DbUtils.GetString(reader, "name"),
                                Date = DbUtils.GetDateTime(reader, "date"),
                                Comment = DbUtils.GetString(reader, "comment"),
                                User = new List<User>()
                            };
                        }
                        
                        gig.User.Add(new User()
                        {
                            Id = DbUtils.GetInt(reader, "UserId"),
                            Name = DbUtils.GetString(reader, "UserName"),
                            Email = DbUtils.GetString(reader, "email")
                        });

                    }

                    reader.Close();
                    return gig;
                }
            }
        }*/

        public Event GetEventWithUsers(int EventId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT event.id AS EventId
                                                ,[event].name
                                                ,[event].date
                                                ,[event].comment
                                                ,[user].name AS UserName
                                                ,[user].email
                                                ,[user].id AS UserId
		                                        ,expense.name AS ExpenseName
		                                        ,expense.amount AS Amount
		                                        ,expense.comment AS ExpenseComment
                                        FROM event
                                        JOIN userEvent ON event.id = userEvent.eventId
                                        JOIN [user] ON [user].id = userEvent.userId
                                        JOIN expense ON expense.eventId = userEvent.eventId
                                        WHERE event.id = @EventId";

                    cmd.Parameters.AddWithValue("@EventId", EventId);

                    var reader = cmd.ExecuteReader();

                    Event gig = null;

                    Dictionary<int, Event> eventDictionary = new Dictionary<int, Event>();

                    while (reader.Read())
                    {
                        int eventId = DbUtils.GetInt(reader, "EventId");

                        if (!eventDictionary.ContainsKey(eventId))
                        {
                            gig = new Event()
                            {
                                Id = eventId,
                                Name = DbUtils.GetString(reader, "name"),
                                Date = DbUtils.GetDateTime(reader, "date"),
                                Comment = DbUtils.GetString(reader, "comment"),
                                User = new List<User>(),
                                Expense = new List<Expense>()
                            };

                            eventDictionary[eventId] = gig;
                        }

                        gig = eventDictionary[eventId];

                        gig.User.Add(new User()
                        {
                            Id = DbUtils.GetInt(reader, "UserId"),
                            Name = DbUtils.GetString(reader, "UserName"),
                            Email = DbUtils.GetString(reader, "email")
                        });

                        gig.Expense.Add(new Expense()
                        {
                            Name = DbUtils.GetString(reader, "ExpenseName"),
                            Amount = DbUtils.GetDecimal(reader, "Amount"),
                            Comment = DbUtils.GetString(reader, "ExpenseComment")
                        });
                    }

                    reader.Close();

                    // Return the event if found, otherwise return null
                    return eventDictionary.ContainsKey(EventId) ? eventDictionary[EventId] : null;
                }
            }
        }
    }
}

﻿using System;
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
    }
}

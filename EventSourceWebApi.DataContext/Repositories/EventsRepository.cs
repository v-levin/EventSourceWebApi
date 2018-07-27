using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EventSourceWebApi.DataContext.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly DbContextOptions<EventSourceDbContext> _dbContext;

        public EventsRepository(DbContextOptions<EventSourceDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public EventResponse GetEvents()
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                return new EventResponse
                {
                    Events = db.Events.ToList()
                };
            }
        }

        public EventResponse GetEvent(int id)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                return new EventResponse
                {
                    Event = db.Events.FirstOrDefault(e => e.Id == id)
                };
            }
        }

        public EventResponse CreateEvent(Event @event)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                db.Events.Add(@event);
                db.SaveChanges();

                return new EventResponse() { EventId = @event.Id };
            }
        }

        public EventResponse UpdateEvent(Event @event)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();

                return new EventResponse() { EventId = @event.Id, Event = @event };
            }
        }

        public Response DeleteEvent(int id)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                var @event = db.Events.Find(id);

                if (@event == null)
                {
                    return new Response()
                    {
                        Result = false,
                        Errors = new List<ResponseError>()
                        {
                            new ResponseError() { Error = $"The Event with Id: { id } was not found." }
                        } 
                    };
                }

                db.Events.Remove(@event);
                db.SaveChanges();

                return new Response();
            }
        }
    }
}

using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
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

        public EventResponse GetEvents(EventRequest request)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                var response = new EventResponse();

                if (string.IsNullOrEmpty(request.Name) &&
                    string.IsNullOrEmpty(request.City) &&
                    string.IsNullOrEmpty(request.Category) &&
                    string.IsNullOrEmpty(request.Location))
                {
                    response.Events = db.Events
                                        .Skip(request.Offset)
                                        .Take(request.Limit)
                                        .ToList();

                    return response;
                }

                FilterEvents(request, db, response);

                response.Events = response.Events
                                          .Skip(request.Offset)
                                          .Take(request.Limit)
                                          .ToList();

                return response;
            }
        }

        private void FilterEvents(EventRequest request, EventSourceDbContext db, EventResponse response)
        {
            if (!string.IsNullOrEmpty(request.Name))
                response.Events = db.Events.Where(e => e.Name.ToLower().Contains(request.Name.ToLower())).ToList();

            if (!string.IsNullOrEmpty(request.City))
            {
                if (HasEvents(response.Events))
                    response.Events = response.Events.Where(e => e.City.ToLower().Contains(request.City.ToLower())).ToList();
                else
                    response.Events = db.Events.Where(e => e.City.ToLower().Contains(request.City.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                if (HasEvents(response.Events))
                    response.Events = response.Events.Where(e => e.Category.ToLower().Contains(request.Category.ToLower())).ToList();
                else
                    response.Events = db.Events.Where(e => e.Category.ToLower().Contains(request.Category.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(request.Location))
            {
                if (HasEvents(response.Events))
                    response.Events = response.Events.Where(e => e.Location.ToLower().Contains(request.Location.ToLower())).ToList();
                else
                    response.Events = db.Events.Where(e => e.Location.ToLower().Contains(request.Location.ToLower())).ToList();
            }
        }

        private bool HasEvents(IList<Event> events)
        {
            return (events != null) ? true : false;
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
                            new ResponseError() { Error = $"The Event with Id: {id} was not found." }
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

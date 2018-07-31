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

        public EventResponse GetEvents(EventSearchRequest searchRequest)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                var response = new EventResponse();

                if (string.IsNullOrEmpty(searchRequest.Name) &&
                    string.IsNullOrEmpty(searchRequest.City) &&
                    string.IsNullOrEmpty(searchRequest.Category) &&
                    string.IsNullOrEmpty(searchRequest.Location))
                {
                    response.Events = db.Events
                                        .Skip(searchRequest.Offset)
                                        .Take(searchRequest.Limit)
                                        .ToList();

                    return response;
                }

                FilterEvents(searchRequest, db, response);

                response.Events = response.Events
                                          .Skip(searchRequest.Offset)
                                          .Take(searchRequest.Limit)
                                          .ToList();

                return response;
            }
        }

        private void FilterEvents(EventSearchRequest searchRequest, EventSourceDbContext db, EventResponse response)
        {
            if (!string.IsNullOrEmpty(searchRequest.Name))
                response.Events = db.Events.Where(e => e.Name.ToLower().Contains(searchRequest.Name.ToLower())).ToList();

            if (!string.IsNullOrEmpty(searchRequest.City))
            {
                if (HasEvents(response.Events))
                    response.Events = response.Events.Where(e => e.City.ToLower().Contains(searchRequest.City.ToLower())).ToList();
                else
                    response.Events = db.Events.Where(e => e.City.ToLower().Contains(searchRequest.City.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(searchRequest.Category))
            {
                if (HasEvents(response.Events))
                    response.Events = response.Events.Where(e => e.Category.ToLower().Contains(searchRequest.Category.ToLower())).ToList();
                else
                    response.Events = db.Events.Where(e => e.Category.ToLower().Contains(searchRequest.Category.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(searchRequest.Location))
            {
                if (HasEvents(response.Events))
                    response.Events = response.Events.Where(e => e.Location.ToLower().Contains(searchRequest.Location.ToLower())).ToList();
                else
                    response.Events = db.Events.Where(e => e.Location.ToLower().Contains(searchRequest.Location.ToLower())).ToList();
            }
        }

        private bool HasEvents(IList<Event> events)
        {
            return events != null;
        }

        public EventResponse GetEvent(IdRequest idRequest)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                return new EventResponse
                {
                    Event = db.Events.FirstOrDefault(e => e.Id == idRequest.Id)
                };
            }
        }

        public EventResponse CreateEvent(PostRequest<Event> postRequest)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                db.Events.Add(postRequest.Payload);
                db.SaveChanges();

                return new EventResponse() { EventId = postRequest.Payload.Id };
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

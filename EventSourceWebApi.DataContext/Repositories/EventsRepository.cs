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

        public EventsResponse GetEvents(EventSearchRequest searchRequest)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                var response = new EventsResponse();

                var events = db.Events.Select(e => e);

                if (!string.IsNullOrEmpty(searchRequest.Name))
                    events = events.Where(e => e.Name.ToLower().StartsWith(searchRequest.Name.ToLower()));

                if (!string.IsNullOrEmpty(searchRequest.City))
                    events = events.Where(e => e.City.ToLower().Contains(searchRequest.City.ToLower()));

                if (!string.IsNullOrEmpty(searchRequest.Category))
                    events = events.Where(e => e.Category.ToLower().Contains(searchRequest.Category.ToLower()));

                if (!string.IsNullOrEmpty(searchRequest.Location))
                    events = events.Where(e => e.Location.ToLower().Contains(searchRequest.Location.ToLower()));

                response.Events = events
                                    .Skip(searchRequest.Offset)
                                    .Take(searchRequest.Limit)
                                    .ToList();

                return response;
            }
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

                return new EventResponse() { Event = postRequest.Payload };
            }
        }

        public EventResponse UpdateEvent(PutRequest<Event> putRequest)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                var @event = db.Events.FirstOrDefault(e => e.Id == putRequest.Id);

                if (@event == null)
                    return new EventResponse() { Result = false };

                db.Entry(putRequest.Payload).State = EntityState.Modified;
                db.SaveChanges();

                return new EventResponse() { Event = putRequest.Payload };
            }
        }

        public Response DeleteEvent(IdRequest idRequest)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                var @event = db.Events.Find(idRequest.Id);

                if (@event == null)
                {
                    return new Response()
                    {
                        Result = false,
                        Errors = new List<ResponseError>()
                        {
                            new ResponseError() { Error = $"The Event with Id: {idRequest.Id} was not found." }
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

using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            Expression<Func<string, string, bool>> startsWithExpression = (e, r) => e.ToLower().StartsWith(r.ToLower());
            Expression<Func<string, string, bool>> containsExpression = (e, r) => e.ToLower().Contains(r.ToLower());
            Func<string, string, bool> startsWith = startsWithExpression.Compile();
            Func<string, string, bool> contains = containsExpression.Compile();

            using (var db = new EventSourceDbContext(_dbContext))
            {
                var events = db.Events.Select(e => e);

                if (!string.IsNullOrEmpty(searchRequest.Name))
                    events = events.Where(e => startsWith(e.Name, searchRequest.Name));

                if (!string.IsNullOrEmpty(searchRequest.City))
                    events = events.Where(e => contains(e.City, searchRequest.City));

                if (!string.IsNullOrEmpty(searchRequest.Category))
                    events = events.Where(e => contains(e.Category, searchRequest.Category));

                if (!string.IsNullOrEmpty(searchRequest.Location))
                    events = events.Where(e => contains(e.Location, searchRequest.Location));

                return new EventsResponse()
                {
                    TotalCount = events.Count(),
                    Events = events
                                .OrderBy(e => e.Name)
                                .Skip(searchRequest.Offset)
                                .Take(searchRequest.Limit)
                                .ToList()
                };
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

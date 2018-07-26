using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;

        public EventsService(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public IEnumerable<Event> GetAll()
        {
            return _eventsRepository.GetAll();
        }
    }
}

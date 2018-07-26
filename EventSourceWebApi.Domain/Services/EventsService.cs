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

        public IEnumerable<Event> GetEvents()
        {
            try
            {
                return _eventsRepository.GetEvents();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Event GetEvent(int id)
        {
            try
            {
                return _eventsRepository.GetEvent(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CreateEvent(Event @event)
        {
            try
            {
                _eventsRepository.CreateEvent(@event);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateEvent(Event @event)
        {
            try
            {
                _eventsRepository.UpdateEvent(@event);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Event Find(int id)
        {
            try
            {
                return _eventsRepository.Find(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteEvent(Event eventToDelete)
        {
            try
            {
                _eventsRepository.DeleteEvent(eventToDelete);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

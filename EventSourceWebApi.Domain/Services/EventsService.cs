using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Messages;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourceWebApi.Domain.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly ILogger _logger;

        public EventsService(IEventsRepository eventsRepository, ILogger logger)
        {
            _eventsRepository = eventsRepository;
            _logger = logger;
        }

        public IEnumerable<Event> GetEvents()
        {
            try
            {
                var events = _eventsRepository.GetEvents();

                if (events.Count() > 0)
                    _logger.Information(LoggingMessages.GetEventsSuccessfully);
                else
                    _logger.Information(LoggingMessages.NoDataInDb);
                
                return events;
            }
            catch (Exception ex)
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

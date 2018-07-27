using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Messages;
using EventSourceWebApi.Contracts.Responses;
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

        public EventResponse GetEvents()
        {
            var response = new EventResponse();

            try
            {
                response = _eventsRepository.GetEvents();

                if (response.Events.Count() > 0)
                    _logger.Information("The Events has successfully taken.");
                else
                    _logger.Information("There is no data in the database.");
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                response.Result = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public EventResponse GetEvent(int id)
        {
            var response = new EventResponse();

            try
            {
                response = _eventsRepository.GetEvent(id);

                if (response.Event == null)
                    _logger.Information(LoggingMessages.EventNotFound(id));
                else
                    _logger.Information($"The Event with Id: {id} has successfully taken.");

                return response;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                response.Result = false;
                return response;
            }
        }

        public EventResponse CreateEvent(Event @event)
        {
            var response = new EventResponse();

            try
            {
                response = _eventsRepository.CreateEvent(@event);
                return response;
            }
            catch (Exception)
            {
                return response;
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

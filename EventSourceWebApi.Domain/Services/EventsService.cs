using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Extensions;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Messages;
using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using EventSourceWebApi.Domain.Validators;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public EventResponse GetEvents(Request request)
        {
            var response = new EventResponse();

            try
            {
                response = _eventsRepository.GetEvents(request);

                if (response.Events.Count() > 0)
                    _logger.Information("The Events has been successfully taken.");
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
                    _logger.Information($"The Event with Id: {id} has been successfully taken.");

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
            if (@event == null)
            {
                return new EventResponse()
                {
                    Result = false,
                    Message = "Value cannot be null."
                };
            }

            var response = new EventsValidator().Validate(@event).ToResponse();

            if (!response.Result)
                return ErrorResponse(response);

            var eventResponse = new EventResponse();

            try
            {
                eventResponse = _eventsRepository.CreateEvent(@event);
                _logger.Information("The Event has been successfully creted.");
                return eventResponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                eventResponse.Result = false;
                eventResponse.Message = ex.Message;
                return eventResponse;
            }
        }

        public EventResponse UpdateEvent(int id, Event @event)
        {
            var response = new EventsValidator().Validate(@event).ToResponse();

            if (!response.Result)
                return ErrorResponse(response);

            var eventResponse = new EventResponse();

            try
            {
                if (id == @event.Id)
                {
                    eventResponse = _eventsRepository.UpdateEvent(@event);
                    _logger.Information($"The Event with Id: {eventResponse.EventId} has been successfully updated.");
                }
                else
                {
                    _logger.Error(LoggingMessages.PassedIdNotMatchWithEventId(id, eventResponse.EventId));
                    eventResponse.Result = false;
                    eventResponse.Message = LoggingMessages.PassedIdNotMatchWithEventId(id, eventResponse.EventId);
                }
                
                return eventResponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                eventResponse.Result = false;
                eventResponse.Message = ex.Message;
                return eventResponse;
            }
        }

        private static EventResponse ErrorResponse(Response response)
        {
            return new EventResponse()
            {
                Result = false,
                Message = string.Join(Environment.NewLine, response.Errors.Select(e => e.Error))
            };
        }

        public Response DeleteEvent(int id)
        {
            var response = new Response();

            try
            {
                return _eventsRepository.DeleteEvent(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                response.Result = false;
                response.Errors = new List<ResponseError>() { new ResponseError() { Error = ex.Message } };
                return response;
            }
        }
    }
}

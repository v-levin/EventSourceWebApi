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

        public EventsResponse GetEvents(EventSearchRequest searchRequest)
        {
            var validator = new EventSearchVallidator().Validate(searchRequest).ToResponse();

            if (!validator.Result)
                return new EventsResponse { Result = false, Errors = validator.Errors };

            var response = new EventsResponse();

            try
            {
                response = _eventsRepository.GetEvents(searchRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                response.Result = false;
                return response;
            }

            return response;
        }

        public EventResponse GetEvent(IdRequest idRequest)
        {
            var validator = new IdRequestValidator().Validate(idRequest).ToResponse();

            if (!validator.Result)
                return new EventResponse { Result = false, Errors = validator.Errors };

            var response = new EventResponse();

            try
            {
                response = _eventsRepository.GetEvent(idRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                response.Result = false;
                return response;
            }

            if (response.Event == null)
            {
                _logger.Information(LoggingMessages.EventNotFound(idRequest.Id));
                response.Result = false;
            }

            return response;
        }

        public EventResponse CreateEvent(PostRequest<Event> postRequest)
        {
            var validator = new EventsValidator().Validate(postRequest.Payload).ToResponse();

            if (!validator.Result)
                return ErrorResponse(validator);

            var eventResponse = new EventResponse();

            try
            {
                eventResponse = _eventsRepository.CreateEvent(postRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                eventResponse.Result = false;
                return eventResponse;
            }

            _logger.Information("The Event has been successfully created.");
            return eventResponse;
        }

        public EventResponse UpdateEvent(PutRequest<Event> putRequest)
        {
            var validator = new UpdateEventValidator().Validate(putRequest).ToResponse();

            if (!validator.Result)
                return ErrorResponse(validator);

            var eventResponse = new EventResponse();
                        
            try
            {
                eventResponse = _eventsRepository.UpdateEvent(putRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                eventResponse.Result = false;
                return eventResponse;
            }

            _logger.Information($"The Event with Id: {eventResponse.Event.Id} has been successfully updated.");
            return eventResponse;
        }

        private static EventResponse ErrorResponse(Response response)
        {
            return new EventResponse()
            {
                Result = false,
                Errors = response.Errors
            };
        }

        public Response DeleteEvent(IdRequest idRequest)
        {
            var validator = new IdRequestValidator().Validate(idRequest).ToResponse();

            if (!validator.Result)
                return new EventResponse { Result = false, Errors = validator.Errors };

            var response = new Response();

            try
            {
                response = _eventsRepository.DeleteEvent(idRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                response.Result = false;
                response.Errors = new List<ResponseError>() { new ResponseError() { Error = ex.Message } };
                return response;
            }

            if (response.Result)
                _logger.Information($"The Event with Id: {idRequest.Id} has been successfully deleted.");
            else
                _logger.Information(LoggingMessages.EventNotFound(idRequest.Id));

            return response;
        }
    }
}

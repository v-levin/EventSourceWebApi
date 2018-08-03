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
                _logger.Error("An exception occurred when getting events", ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "GetEventsException", Error = "An exception occurred when getting events" });
                return response;
            }

            return response;
        }

        public EventResponse GetEvent(EventIdRequest idRequest)
        {
            var validator = new EventIdRequestValidator(_eventsRepository).Validate(idRequest).ToResponse();

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

            return eventResponse;
        }

        public EventResponse UpdateEvent(PutRequest<Event> putRequest)
        {
            var validator = new UpdateEventValidator(_eventsRepository).Validate(putRequest).ToResponse();

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

            if (eventResponse.Result)
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

        public Response DeleteEvent(EventIdRequest idRequest)
        {
            var validator = new EventIdRequestValidator(_eventsRepository).Validate(idRequest).ToResponse();

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

            return response;
        }
    }
}

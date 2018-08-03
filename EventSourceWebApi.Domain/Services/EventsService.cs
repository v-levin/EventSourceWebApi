using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Extensions;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Messages;
using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using EventSourceWebApi.Domain.Validators;
using Serilog;
using System;

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
                _logger.Error(ExceptionMessages.GetEventsException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "GetEventsException", Error = ExceptionMessages.GetEventsException });
                return response;
            }

            return response;
        }

        public EventResponse GetEvent(EventIdRequest idRequest)
        {
            var validator = new Response();
            var response = new EventResponse();

            try
            {
                validator = new EventIdRequestValidator(_eventsRepository).Validate(idRequest).ToResponse();
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.GetEventException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "GetEventException", Error = ExceptionMessages.GetEventException });
                return response;
            }

            if (!validator.Result)
                return new EventResponse() { Errors = validator.Errors };

            try
            {
                response = _eventsRepository.GetEvent(idRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.GetEventException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "GetEventException", Error = ExceptionMessages.GetEventException });
                return response;
            }

            return response;
        }

        public EventResponse CreateEvent(PostRequest<Event> postRequest)
        {
            var response = new EventResponse();

            if (postRequest.Payload != null)
            {
                var validator = new EventsValidator().Validate(postRequest.Payload).ToResponse();

                if (!validator.Result)
                    return ErrorResponse(validator);

                try
                {
                    return _eventsRepository.CreateEvent(postRequest);
                }
                catch (Exception ex)
                {
                    _logger.Error(ExceptionMessages.CreateEventException, ex);
                    response.Result = false;
                    response.Errors.Add(new ResponseError { Name = "CreateEventException", Error = ExceptionMessages.CreateEventException });
                    return response;
                }
            }

            _logger.Information(ExceptionMessages.NullObject);
            response.Result = false;
            response.Errors.Add(new ResponseError() { Name = "NullObject", Error = ExceptionMessages.NullObject });
            return response;
        }

        public EventResponse UpdateEvent(PutRequest<Event> putRequest)
        {
            var validator = new Response();
            var response = new EventResponse();

            try
            {
                validator = new UpdateEventValidator(_eventsRepository).Validate(putRequest).ToResponse();
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.UpdateEventException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "UpdateEventException", Error = ExceptionMessages.UpdateEventException });
                return response;
            }

            if (!validator.Result)
                return ErrorResponse(validator);

            try
            {
                response = _eventsRepository.UpdateEvent(putRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.UpdateEventException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "UpdateEventException", Error = ExceptionMessages.UpdateEventException });
                return response;
            }

            if (response.Result)
                _logger.Information($"The Event with Id: {response.Event.Id} has been successfully updated.");

            return response;
        }

        public Response DeleteEvent(EventIdRequest idRequest)
        {
            var validator = new Response();
            var response = new Response();

            try
            {
                validator = new EventIdRequestValidator(_eventsRepository).Validate(idRequest).ToResponse();
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.DeleteEventException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "DeleteEventException", Error = ExceptionMessages.DeleteEventException });
                return response;
            }

            if (!validator.Result)
                return new Response() { Errors = validator.Errors };

            try
            {
                return _eventsRepository.DeleteEvent(idRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.DeleteEventException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "DeleteEventException", Error = ExceptionMessages.DeleteEventException });
                return response;
            }
        }

        private static EventResponse ErrorResponse(Response response)
        {
            return new EventResponse()
            {
                Result = false,
                Errors = response.Errors
            };
        }
    }
}

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
    public class PlacesService : IPlacesService
    {
        private readonly IPlacesRepository _placeRepository;
        private readonly ILogger _logger;

        public PlacesService(IPlacesRepository placeRepository, ILogger logger)
        {
            _placeRepository = placeRepository;
            _logger = logger;
        }

        public PlacesResponse GetAllPlaces(PlaceSearchRequest request)
        {
            var validator = new PlaceSearchValidator().Validate(request).ToResponse();

            if (!validator.Result)
                return new PlacesResponse { Errors = validator.Errors, Result = false };

            var response = new PlacesResponse();

            try
            {
                response = _placeRepository.GetAllPlaces(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.GetPlacesException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "GetEventsException", Error = ExceptionMessages.GetPlacesException });
                return response;
            }

            return response;
        }

        public PlaceResponse GetPlace(PlaceIdRequest request)
        {
            var response = new PlaceResponse();
            var validator = new Response();

            try
            {
                validator = new PlaceIdRequestValidator(_placeRepository).Validate(request).ToResponse();
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.GetPlaceException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "GetPlaceException", Error = ExceptionMessages.GetPlaceException });
                return response;
            }

            if (!validator.Result)
                return new PlaceResponse() { Errors = validator.Errors };

            try
            {
                response = _placeRepository.GetPlace(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.GetPlaceException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "GetPlaceException", Error = ExceptionMessages.GetPlaceException });
                return response;
            }

            return response;
        }

        public PlaceResponse CreatePlace(PostRequest<Place> request)
        {
            var response = new PlaceResponse();

            if (request.Payload != null)
            {
                var validator = new PlacesValidator().Validate(request.Payload).ToResponse();

                if (!validator.Result)
                    return new PlaceResponse { Errors = validator.Errors, Result = false };

                try
                {
                    return _placeRepository.CreatePlace(request);
                }
                catch (Exception ex)
                {
                    _logger.Error(ExceptionMessages.CreatePlaceException, ex);
                    response.Result = false;
                    response.Errors.Add(new ResponseError { Name = "CreatePlaceException", Error = ExceptionMessages.CreatePlaceException });
                    return response;
                }
            }

            _logger.Information(ExceptionMessages.NullObject);
            response.Result = false;
            response.Errors.Add(new ResponseError() { Name = "NullObject", Error = ExceptionMessages.NullObject });
            return response;
        }

        public PlaceResponse UpdatePlace(PutRequest<Place> request)
        {
            var validator = new Response();
            var response = new PlaceResponse();

            try
            {
                validator = new UpdatePlaceValidator(_placeRepository).Validate(request).ToResponse();
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.UpdatePlaceException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "UpdatePlaceException", Error = ExceptionMessages.UpdatePlaceException });
                return response;
            }

            if (!validator.Result)
                return new PlaceResponse { Errors = validator.Errors, Result = false };

            try
            {
                response = _placeRepository.UpdatePlace(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.UpdatePlaceException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "UpdateEventException", Error = ExceptionMessages.UpdatePlaceException });
                return response;
            }

            if (response.Result)
                _logger.Information($"The Place with Id: {response.Place.Id} has been successfully updated.");

            return response;
        }

        public Response DeletePlace(PlaceIdRequest request)
        {
            var validator = new Response();
            var response = new Response();

            try
            {
                validator = new PlaceIdRequestValidator(_placeRepository).Validate(request).ToResponse();
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.DeleteEventException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "DeletePlaceException", Error = ExceptionMessages.DeletePlaceException });
                return response;
            }

            if (!validator.Result)
                return new Response() { Errors = validator.Errors };

            try
            {
                return _placeRepository.DeletePlace(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ExceptionMessages.DeletePlaceException, ex);
                response.Result = false;
                response.Errors.Add(new ResponseError { Name = "DeletePlaceException", Error = ExceptionMessages.DeletePlaceException });
                return response;
            }
        }
    }
}

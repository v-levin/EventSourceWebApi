using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Extensions;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using EventSourceWebApi.Domain.Validators;
using Serilog;
using System;

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
            {
                _logger.Error($"Invalid request {request}");
                return new PlacesResponse { Errors = validator.Errors, Result = false };
            }

            try
            {
                return _placeRepository.GetAllPlaces(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PlacesResponse { Result = false };
            }
        }

        public PlaceResponse GetPlace(IdRequest request)
        {
            var placeResponse = new PlaceResponse();
            var validator = new IdRequestValidator().Validate(request).ToResponse();

            if (!validator.Result)
            {
                _logger.Error($"Invalid request for the id: {request.Id }");
                return new PlaceResponse() { Result = false, Errors = validator.Errors };
            }
            try
            {
                return _placeRepository.GetPlace(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PlaceResponse() { Result = false };
            }
        }

        public PlaceResponse CreatePlace(PostRequest<Place> request)
        {
            var response = new PlacesValidator().Validate(request.Payload).ToResponse();

            if (!response.Result)
            {
                _logger.Error($"Invalid request {request}");
                return new PlaceResponse { Errors = response.Errors, Result = false };
            }
            try
            {
                return _placeRepository.CreatePlace(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PlaceResponse() { Result = false };
            }
        }

        public PlaceResponse UpdatePlace(PutRequest<Place> request)
        {
            var validator = new UpdatePlaceValidator().Validate(request).ToResponse(); 
            
            if (!validator.Result)
            {
                _logger.Error($"Invalid request {request}");
                return new PlaceResponse { Errors = validator.Errors, Result = false };
            }
            try
            {
                return _placeRepository.UpdatePlace(request);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PlaceResponse() { Result = false };
            }
        }

        public Response DeletePlace(IdRequest request)
        {
            var response = new Response();
            var validator = new IdRequestValidator().Validate(request).ToResponse();

            if (!validator.Result)
            {
                _logger.Error($"Invalid request for the id: {request.Id }");
                return new Response() { Result = false, Errors = validator.Errors };
            }
            try
            {
                return _placeRepository.DeletePlace(request);
               
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new Response() { Result = false };
            }
        }
    }
}

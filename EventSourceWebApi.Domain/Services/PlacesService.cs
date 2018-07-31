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
            try 
            {
                placeResponse = _placeRepository.GetPlace(request);
                if (placeResponse.Place == null)
                {
                    _logger.Error($"The place with id:{request.Id} doesn't exist.");
                    placeResponse.Result = false;
                    return placeResponse;
                }
                return placeResponse;
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
            try
            {
                var response = new PlacesValidator().Validate(request.Payload).ToResponse();
                if (!response.Result)
                {
                    return new PlaceResponse { Errors = response.Errors, Result = false };
                }
                return new PlaceResponse() { Place  = _placeRepository.UpdatePlace(request).Place};
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
            try
            {
                var deletePlaceResponse = _placeRepository.DeletePlace(request);
                if (!deletePlaceResponse.Result)
                {
                    _logger.Error($"Unable to delete place with id: {request.Id}");
                    response.Result = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PlaceResponse() { Result = false };
            }
        }
    }
}

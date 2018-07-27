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

        public PlaceResponse GetAllPlaces(Request placeRequest)
        {
            try
            {
                return _placeRepository.GetAllPlaces(placeRequest);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PlaceResponse { Result = false };
            }
        }

        public PlaceResponse GetPlace(int id)
        {
            var placeResponse = new PlaceResponse();
            try
            {
                placeResponse = _placeRepository.GetPlace(id);
                if (placeResponse.Place == null)
                {
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

        public PlaceResponse CreatePlace(Place place)
        {
            try
            {
                var response = new PlacesValidator().Validate(place).ToResponse();

                if (!response.Result)
                {
                    return new PlaceResponse { Errors = response.Errors, Result = false };
                }
                return new PlaceResponse() { PlaceId = _placeRepository.CreatePlace(place).PlaceId };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PlaceResponse() { Result = false };
            }
        }

        public PlaceResponse UpdatePlace(Place place, int id)
        {
            try
            {
                var response = new PlacesValidator().Validate(place).ToResponse();
                if (!response.Result)
                {
                    return new PlaceResponse { Errors = response.Errors, Result = false };
                }
                return new PlaceResponse() { PlaceId = _placeRepository.UpdatePlace(place, id).PlaceId };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new PlaceResponse() { Result = false };
            }
        }

        public Response DeletePlace(int id)
        {
            var response = new Response();
            try
            {
                var deletePlaceResponse = _placeRepository.DeletePlace(id);
                if (!deletePlaceResponse.Result)
                {
                    _logger.Error($"Unable to delete place with id: {id}");
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

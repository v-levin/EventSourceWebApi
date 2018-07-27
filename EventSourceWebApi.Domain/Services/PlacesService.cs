using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Extensions;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using EventSourceWebApi.Domain.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Services
{
    public class PlacesService : IPlacesService
    {
        private readonly IPlacesRepository _placeRepository;

        public PlacesService(IPlacesRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public PlaceResponse GetAllPlaces(Request placeRequest)
        {
            try
            {
                return _placeRepository.GetAllPlaces(placeRequest);
            }
            catch (Exception ex)
            {
                return new PlaceResponse { Message = ex.Message};
            }
        }

        public PlaceResponse GetPlace(int id)
        {
            var  getPlaceResponse = new PlaceResponse();
            try
            {
                var _place = _placeRepository.GetPlace(id);
                getPlaceResponse.Place = _place.Place;
                return getPlaceResponse;
            }
            catch (Exception ex)
            {
                getPlaceResponse.Message = ex.Message;
                return getPlaceResponse;
            }
        }

        //todo: pretty please put this as response
        public PlaceResponse CreatePlace(Place place)
        {
            var response = new PlacesValidator().Validate(place).ToResponse();

            if (!response.Result)
                return response as PlaceResponse;

            try
            {
                _placeRepository.CreatePlace(place); //todo => Create
                return new PlaceResponse(); //todo new reponse
            }
            catch (Exception ex)
            {
                //logiranje response false
                //isValidPlace.Result = false;
                //return isValidPlace;
                throw;
            }
        }

        public PlaceResponse UpdatePlace(Place place, int id)
        {
            _placeRepository.UpdatePlace(place, id);
            return new PlaceResponse();
        }

        public Response DeletePlace(int id)
        {
            var isDeleted = _placeRepository.DeletePlace(id);
            return new Response();
        }
    }
}

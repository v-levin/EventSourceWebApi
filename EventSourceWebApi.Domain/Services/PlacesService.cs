using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Extensions;
using EventSourceWebApi.Contracts.Interfaces;
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

        public Response Delete(int id)
        {
            var isDeleted = _placeRepository.Delete(id);
            return new Response();
        }

        public PlaceResponse Update(Place place, int id)
        {
            _placeRepository.Edit(place, id);
            return new  PlaceResponse();
        }

        public PlaceResponse Get(int id)
        {
            var _getPlaceResponse = new PlaceResponse();
            try
            {
                var _place = _placeRepository.Get(id);
                _getPlaceResponse.Place = _place;
                return _getPlaceResponse;
            }
            catch (Exception ex)
            {
                _getPlaceResponse.Message = ex.Message;
                return _getPlaceResponse;
            }
        }


        //todo: pretty please put this as response
        public IEnumerable<Place> GetAll()
        {
            try
            {
                return _placeRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PlaceResponse Create(Place place)
        {
            var response = new PlacesValidator().Validate(place).ToResponse();

            if (!response.Result)
                return response as PlaceResponse;

            try
            {
                _placeRepository.Save(place); //todo => Create
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
    }
}

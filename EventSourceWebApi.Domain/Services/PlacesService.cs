using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Responses;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Domain.Services
{
    public class PlacesService : IPlacesService
    {
        private readonly IPlacesRepository _placeRepository;
        private IPlaceValidator _placeValidator;

        public PlacesService(IPlacesRepository placeRepository, IPlaceValidator placeValidator)
        {
            _placeRepository = placeRepository;
            _placeValidator = placeValidator;

        }

        public bool Delete(int id)
        {
            var isDeleted = _placeRepository.Delete(id);
            return isDeleted;
        }

        public void Edit(Place place, int id)
        {
            _placeRepository.Edit(place, id);
        }

        public GetPlaceResponse Get(int id)
        {
            var _getPlaceResponse = new GetPlaceResponse();
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

        public ValdatePlaceResponse Save(Place place)
        {
            var isValidPlace  = new ValdatePlaceResponse();
            try
            {
                 isValidPlace = _placeValidator.ValidPlace(place);
                if(!isValidPlace.Result)
                {
                    return isValidPlace;
                }
                _placeRepository.Save(place);
                return isValidPlace;
            }
            catch (Exception ex)
            {
                isValidPlace.Result = false;
                return isValidPlace;
            }
        }
    }
}

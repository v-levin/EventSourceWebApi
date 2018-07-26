using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
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

        public bool Delete(int id)
        {
            var isDeleted = _placeRepository.Delete(id);
            return isDeleted;
        }

        public void Edit(Place place, int id)
        {
            _placeRepository.Edit(place, id);
        }

        public Place Get(int id)
        {
            return _placeRepository.Get(id);
        }

        public IEnumerable<Place> GetAll()
        {
            return _placeRepository.GetAll();
        }

        public void Save(Place place)
        {
            _placeRepository.Save(place);
        }
    }
}

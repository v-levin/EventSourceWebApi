using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlacesService
    {
        IEnumerable<Place> GetAll();

        GetPlaceResponse Get(int id);

        ValdatePlaceResponse Save(Place place);

        void Edit(Place place, int id);

        bool Delete(int id);
    }
}

using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlacesRepository
    {
        PlaceResponse GetAllPlaces(PlaceRequest placeRequest);

        PlaceResponse GetPlace(int id);

        PlaceResponse CreatePlace(Place place);

        PlaceResponse UpdatePlace(Place place, int id);

        Response DeletePlace(int id);
    }
}

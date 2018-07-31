using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlacesRepository
    {
        PlacesResponse GetAllPlaces(PlaceSearchRequest placeRequest);

        PlaceResponse GetPlace(IdRequest id);

        PlaceResponse CreatePlace(PostRequest<Place> Place);

        PlaceResponse UpdatePlace(PutRequest<Place> Place);

        Response DeletePlace(IdRequest id);
    }
}

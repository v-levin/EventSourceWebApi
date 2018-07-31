using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlacesService
    {
        PlaceResponse GetAllPlaces(PlaceSearchRequest placeRequest); //todo GetAllPlacesResponse //search, paging?! limit. MAX LIMIT = 1000 (search, consider nwew method)

        PlaceResponse GetPlace(IdRequest id); 

        PlaceResponse CreatePlace(PostRequest<Place> place); 

        PlaceResponse UpdatePlace(Place place, int id); 

        Response DeletePlace(IdRequest id); 
    }
}

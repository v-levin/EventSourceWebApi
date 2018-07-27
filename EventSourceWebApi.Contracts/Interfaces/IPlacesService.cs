using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlacesService
    {
        PlaceResponse GetAllPlaces(Request placeRequest); //todo GetAllPlacesResponse //search, paging?! limit. MAX LIMIT = 1000 (search, consider nwew method)

        PlaceResponse GetPlace(int id); 

        PlaceResponse CreatePlace(Place place); 

        PlaceResponse UpdatePlace(Place place, int id); 

        Response DeletePlace(int id); 
    }
}

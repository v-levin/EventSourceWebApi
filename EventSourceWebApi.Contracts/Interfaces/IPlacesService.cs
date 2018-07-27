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

        PlaceResponse GetPlace(int id); //todo GetPlace

        PlaceResponse CreatePlace(Place place); //todo CretePlaceResponse (int) => 201 (int creaed)

        PlaceResponse UpdatePlace(Place place, int id); //todo Response 200

        Response DeletePlace(int id); //Response 200 
    }
}

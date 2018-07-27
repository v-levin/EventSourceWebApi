using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlacesService
    {
        IEnumerable<Place> GetAll(); //todo GetAllPlacesResponse //search, paging?! limit. MAX LIMIT = 1000 (search, consider nwew method)

        PlaceResponse Get(int id); //todo GetPlace

        PlaceResponse Create(Place place); //todo CretePlaceResponse (int) => 201 (int creaed)

        PlaceResponse Update(Place place, int id); //todo Response 200

        Response Delete(int id); //Response 200
    }
}

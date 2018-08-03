using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlacesRepository
    {
        PlacesResponse GetAllPlaces(PlaceSearchRequest placeRequest);

        PlaceResponse GetPlace(PlaceIdRequest id);

        PlaceResponse CreatePlace(PostRequest<Place> Place);

        PlaceResponse UpdatePlace(PutRequest<Place> Place);

        Response DeletePlace(PlaceIdRequest id);
    }
}

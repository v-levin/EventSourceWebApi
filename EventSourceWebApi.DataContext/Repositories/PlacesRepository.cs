using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EventSourceWebApi.DataContext.Repositories
{
    public class PlacesRepository : IPlacesRepository
    {
        private readonly DbContextOptions<EventSourceDbContext> _contextOptions;

        public PlacesRepository(DbContextOptions<EventSourceDbContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public PlaceResponse GetAllPlaces(PlaceSearchRequest placeRequest)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var placeResponse = new PlaceResponse();

                if (string.IsNullOrEmpty(placeRequest.Name) && string.IsNullOrEmpty(placeRequest.City) && string.IsNullOrEmpty(placeRequest.Location))
                {

                    placeResponse.Places = db.Places
                         .Skip(placeRequest.Offset)
                                    .Take(placeRequest.Limit)
                                     .ToList();

                    return placeResponse;
                }

                FilterPlaces(placeRequest, db, placeResponse);

                placeResponse.Places = placeResponse.Places
                     .Skip(placeRequest.Offset)
                                     .Take(placeRequest.Limit)
                                      .ToList();

                return placeResponse;

            }
        }

        private static void FilterPlaces(PlaceSearchRequest placeRequest, EventSourceDbContext db, PlaceResponse placeResponse)
        {
            if (!string.IsNullOrEmpty(placeRequest.Name))
                placeResponse.Places = db.Places
                            .Where(p => p.Name.Contains(placeRequest.Name.ToLower()))
                            .ToList();
            var list2 = placeResponse.Places; 
            if (!string.IsNullOrEmpty(placeRequest.Location))
            {
                if (placeResponse.Places != null)
                    placeResponse.Places = placeResponse.Places
                           .Where(p => p.Location.Contains(placeRequest.Location.ToLower()))
                           .ToList();
                else
                    placeResponse.Places = db.Places
                            .Where(p => p.Location.Contains(placeRequest.Location.ToLower()))
                            .ToList();
            }
            var list1 = placeResponse.Places;
            if (!string.IsNullOrEmpty(placeRequest.City))
            {
                if (placeResponse.Places != null)
                    placeResponse.Places = placeResponse.Places
                           .Where(p => p.City.Contains(placeRequest.City.ToLower()))
                           .ToList();
                else
                    placeResponse.Places = db.Places
                           .Where(p => p.City.Contains(placeRequest.City.ToLower()))
                           .ToList();
                var list = placeResponse.Places;
            }
        }

        public PlaceResponse GetPlace(IdRequest request)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                return new PlaceResponse
                {
                    Place = db.Places.Where(p => p.Id == request.Id)
                    .FirstOrDefault()
                };
            }

        }

        public PlaceResponse CreatePlace(PostRequest<Place> request)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var newPlace = new Place()
                {
                    DateRegistered = request.Payload.DateRegistered,
                    Capacity = request.Payload.Capacity,
                    Location = request.Payload.Location.ToLower(),
                    Name = request.Payload.Name.ToLower(),
                    Description = request.Payload.Description,
                    City = request.Payload.City.ToLower()
                };
                db.Places.Add(newPlace);
                db.SaveChanges();
                return new PlaceResponse() { PlaceId = newPlace.Id };
            }
        }

        public PlaceResponse UpdatePlace(PutRequest<Place> request)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var _place = db.Places.Find(request.Id);

                if (_place != null)
                {
                    _place.DateRegistered = request.Payload.DateRegistered;
                    _place.Description = request.Payload.Description;
                    _place.City = request.Payload.City.ToLower();
                    _place.Capacity = request.Payload.Capacity;
                    _place.Name = request.Payload.Name.ToLower();
                    _place.Location = request.Payload.Location.ToLower();

                    db.Places.Attach(request.Payload);
                    db.SaveChanges();
                }
                return new PlaceResponse() { PlaceId = _place.Id };
            }
        }

        public Response DeletePlace(IdRequest request)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var _place = db.Places.Find(request.Id);

                if (_place == null)
                {
                    return new Response() { Result = false };
                }
                db.Places.Remove(_place);
                db.SaveChanges();
                return new Response() { Result = true };
            }
        }
    }
}

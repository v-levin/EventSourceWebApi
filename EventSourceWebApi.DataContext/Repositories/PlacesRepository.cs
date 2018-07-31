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

        public PlacesResponse GetAllPlaces(PlaceSearchRequest placeRequest)
        {
            var placesResponse = new PlacesResponse();
           
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                IQueryable<Place> getPlacesQuery = db.Places;
                if (!string.IsNullOrEmpty(placeRequest.Name))
                    getPlacesQuery = db.Places.Where(p => p.Name.StartsWith(placeRequest.Name.ToLower()));
                
                if (!string.IsNullOrEmpty(placeRequest.Location))
                    getPlacesQuery = db.Places.Where(p => p.Location.Contains(placeRequest.Location.ToLower()));
             
                if(!string.IsNullOrEmpty(placeRequest.City))
                    getPlacesQuery = db.Places.Where(p => p.City.Contains(placeRequest.City.ToLower()));

                var places = getPlacesQuery
                    .Skip(placeRequest.Offset)
                    .Take(placeRequest.Limit)
                    .OrderBy(p => p.Name)
                    .ToList();

                return new PlacesResponse() { Places = places };

            }
        }

        public PlaceResponse GetPlace(IdRequest request)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var place = db.Places.Find(request.Id);
                if (place != null)
                {
                    return new PlaceResponse() { Place = place, Result = true };
                }
                return new PlaceResponse() { Place = place, Result = false };
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
                return new PlaceResponse() { Place = newPlace, };
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
                return new PlaceResponse() { Place = _place };
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

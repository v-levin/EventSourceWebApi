using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

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

            Expression<Func<Place, bool>> checkName = p => p.Name.StartsWith(placeRequest.Name);
            Expression<Func<Place, bool>> checkLocation = p => p.Location.Contains(placeRequest.Location);
            Expression<Func<Place, bool>> checkCity = p => p.City.Contains(placeRequest.City);

            using (var db = new EventSourceDbContext(_contextOptions))
            {
                IQueryable<Place> placeQuery = db.Places;
                placeQuery = !string.IsNullOrEmpty(placeRequest.Name) ? placeQuery.Where(checkName) :
                             !string.IsNullOrEmpty(placeRequest.City) ? placeQuery.Where(checkCity) :
                             !string.IsNullOrEmpty(placeRequest.Location) ? placeQuery.Where(checkLocation) : placeQuery;


                var places = placeQuery
                    .Skip(placeRequest.Offset)
                    .Take(placeRequest.Limit)
                    .OrderBy(p => p.Name)
                    .ToList();

                return new PlacesResponse { Places = places, TotalPlaces = places.Count() };

            }

        }

        public PlaceResponse GetPlace(IdRequest request)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var place = db.Places.Find(request.Id);
                if (place != null)
                {
                    return new PlaceResponse() { Place = place };
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
                var response = new PlaceResponse();
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
                    response.Place = _place;
                    return response;
                }
                response.Result = false;
                return response;
            }
        }

        public Response DeletePlace(IdRequest request)
        {
            var response = new Response();
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var _place = db.Places.Find(request.Id);

                if (_place == null)
                {
                    response.Result = false;
                    return response;
                }
                db.Places.Remove(_place);
                db.SaveChanges();
                return response;
            }
        }
    }
}

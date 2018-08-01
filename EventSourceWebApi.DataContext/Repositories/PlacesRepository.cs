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

            Expression<Func<string, string, bool>> startsWith = (p, e) => p.ToLower().StartsWith(e.ToLower());
            Expression<Func<string, string, bool>> contains = (p, e) => p.ToLower().Contains(e.ToLower());

            using (var db = new EventSourceDbContext(_contextOptions))
            {
                IQueryable<Place> placeQuery = db.Places;
                placeQuery = !string.IsNullOrEmpty(placeRequest.Name) ? placeQuery.Where(p => startsWith.Compile()(p.Name, placeRequest.Name)) :
                             !string.IsNullOrEmpty(placeRequest.City) ? placeQuery.Where(p => contains.Compile()(p.City, placeRequest.City)) :
                             !string.IsNullOrEmpty(placeRequest.Location) ? placeQuery.Where(p => contains.Compile()(p.Location, placeRequest.Location)) : placeQuery;


                var places = placeQuery
                    .OrderBy(p => p.Name)
                    .Skip(placeRequest.Offset)
                    .Take(placeRequest.Limit)
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

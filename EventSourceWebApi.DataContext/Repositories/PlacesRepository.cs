using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourceWebApi.DataContext.Repositories
{
    public class PlacesRepository : IPlacesRepository
    {
        private readonly DbContextOptions<EventSourceDbContext> _contextOptions;

        public PlacesRepository(DbContextOptions<EventSourceDbContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public PlaceResponse GetAllPlaces(Request placeRequest)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                return new PlaceResponse()
                {
                    Places = db.Places.Where(s => s.Name.Contains(placeRequest.Keyword) || s.City.Contains(placeRequest.Keyword))
                };
            }
        }

        public PlaceResponse GetPlace(int id)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                return new PlaceResponse
                {
                    Place = db.Places.Where(p => p.Id == id)
                    .FirstOrDefault()
                };
            }

        }
        public PlaceResponse CreatePlace(Place place)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var newPlace = new Place()
                {
                    DateRegistered = place.DateRegistered,
                    Capacity = place.Capacity,
                    Location = place.Location,
                    Name = place.Name,
                    Description = place.Description,
                    City = place.City
                };
                db.Places.Add(newPlace);
                db.SaveChanges();
                return new PlaceResponse() { PlaceId = newPlace.Id };
            }
        }

        public PlaceResponse UpdatePlace(Place place, int id)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var _place = db.Places.Find(id);

                if (_place != null)
                {
                    _place.DateRegistered = place.DateRegistered;
                    _place.Description = place.Description;
                    _place.City = place.City;
                    _place.Capacity = place.Capacity;
                    _place.Name = place.Name;
                    _place.Location = place.Location;

                    db.Places.Attach(place);
                    db.SaveChanges();
                }
                return new PlaceResponse() { PlaceId = _place.Id };
            }
        }

        public Response DeletePlace(int id)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var _place = db.Places.Find(id);

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

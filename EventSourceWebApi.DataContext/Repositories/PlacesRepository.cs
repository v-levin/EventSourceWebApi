using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
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

        public bool Delete(int id)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var _place = db.Places.Find(id);

                if (_place == null)
                {
                    return false;
                }
                db.Places.Remove(_place);
                db.SaveChanges();
                return true;
            }
        }

        public void Edit(Place place, int id)
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
            }
        }

        public Place Get(int id)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var _place = db.Places.Where(p => p.Id == id).FirstOrDefault();

                return _place;

            }
        }

        public IEnumerable<Place> GetAll()
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                return db.Places.ToList();
            }
        }

        public void Save(Place place)
        {
            using (var db = new EventSourceDbContext(_contextOptions))
            {
                var _place = new Place()
                {
                    DateRegistered = place.DateRegistered,
                    Capacity = place.Capacity,
                    Location = place.Location,
                    Name = place.Name,
                    Description = place.Description,
                    City = place.City
                };
                db.Places.Add(_place);
                db.SaveChanges();
            }
        }
    }
}

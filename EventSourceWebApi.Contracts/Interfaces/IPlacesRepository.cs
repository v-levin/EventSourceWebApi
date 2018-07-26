using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IPlacesRepository
    {
        IEnumerable<Place> GetAll();

        Place Get(int id);

        void Save(Place place);

        void Edit(Place place, int id);

        bool Delete(int id);
    }
}

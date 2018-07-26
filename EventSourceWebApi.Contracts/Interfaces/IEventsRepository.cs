using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsRepository
    {
        IEnumerable<Event> GetEvents();
        Event GetEvent(int id);
    }
}

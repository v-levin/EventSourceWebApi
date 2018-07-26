using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourceWebApi.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        public IEnumerable<Event> GetAll()
        {
            var allEvents = _eventsService.GetAll();

            return allEvents;
        }
    }
}

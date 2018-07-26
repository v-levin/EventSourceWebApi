using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Messages;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
        private readonly ILogger _logger;

        public EventsController(IEventsService eventsService, ILogger logger)
        {
            _eventsService = eventsService;
            _logger = logger;
        }

        /// <summary>
        /// Returns the Event Collection.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Event> GetEvents()
        {
            _logger.Information(LoggingMessages.GettingAllEvents);
            return _eventsService.GetEvents();
        }


        /// <summary>
        /// Returns an individual Event.
        /// </summary>
        /// <param name="id">The Event id.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            _logger.Information(LoggingMessages.GettingEventById(id));
            var eventById = _eventsService.GetEvent(id);

            return Ok(eventById);
        }
    }
}

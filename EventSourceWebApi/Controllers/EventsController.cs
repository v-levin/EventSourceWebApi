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
        public IActionResult GetEvents()
        {
            _logger.Information(LoggingMessages.GettingAllEvents);
            var events = _eventsService.GetEvents();

            _logger.Information(LoggingMessages.GetEventsSuccessfully);
            return Ok(events);
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
            var @event = _eventsService.GetEvent(id);

            if (@event == null)
            {
                _logger.Error(LoggingMessages.EventNotFound(id));
                return NotFound(@event);
            }

            _logger.Information(LoggingMessages.GetEventSuccessfully(id));
            return Ok(@event);
        }

        /// <summary>
        /// Creates a new Event.
        /// </summary>
        /// <param name="event">The Event object.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostEvent([FromBody]Event @event)
        {
            _logger.Information(LoggingMessages.CreatingEvent);
            if (!ModelState.IsValid)
            {
                _logger.Error(LoggingMessages.DataNotValid);
                return BadRequest(ModelState);
            }

            _eventsService.CreateEvent(@event);
            _logger.Information(LoggingMessages.EventSuccessfullyCreated);
            return Ok(@event);
        }

        /// <summary>
        /// Updates an individual Event.
        /// </summary>
        /// <param name="id">The Event id.</param>
        /// <param name="event">The Event object.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult PutEvent(int id, [FromBody]Event @event)
        {
            _logger.Information(LoggingMessages.EditingEventById(id));
            if (!ModelState.IsValid)
            {
                _logger.Error(LoggingMessages.DataNotValid);
                return BadRequest(ModelState);
            }

            if (id != @event.Id)
            {
                _logger.Error(LoggingMessages.PassedIdNotMatchWithEventId(id, @event.Id));
                return BadRequest(LoggingMessages.IdsNotMatch);
            }

            _eventsService.UpdateEvent(@event);
            _logger.Information(LoggingMessages.EventSuccessfullyModified(id));
            return Ok(@event);
        }

        /// <summary>
        /// Deletes an individual Event.
        /// </summary>
        /// <param name="id">The Event id.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            _logger.Information(LoggingMessages.LookingForEventToDelete(id));
            var eventToDelete = _eventsService.Find(id);

            if (eventToDelete == null)
            {
                _logger.Error(LoggingMessages.EventNotFound(id));
                return NotFound(eventToDelete);
            }

            _eventsService.DeleteEvent(eventToDelete);
            _logger.Information(LoggingMessages.EventSuccessfullyDeleted(id));
            return NoContent();
        }
    }
}

using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Messages;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        public IActionResult GetEvents(string name, string city, string category, string location)
        {
            var searchRequest = new EventSearchRequest()
            {
                Name = name,
                City = city,
                Category = category,
                Location = location
            };

            var response = _eventsService.GetEvents(searchRequest);

            if (!response.Result)
            {
                return BadRequest(response.Errors);
            }
            
            return Ok(response.Events);
        }
        
        /// <summary>
        /// Returns an individual Event.
        /// </summary>
        /// <param name="id">The Event id.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            var idRequest = new IdRequest() { Id = id };
            _logger.Information($"Getting Event with Id: {idRequest.Id}.");
            var response = _eventsService.GetEvent(idRequest);

            if (!response.Result)
            {
                return NotFound(response.Event);
            }
            
            return Ok(response.Event);
        }

        /// <summary>
        /// Creates a new Event.
        /// </summary>
        /// <param name="event">The Event object.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostEvent([FromBody]Event @event)
        {
            var response = _eventsService.CreateEvent(@event);

            if (!response.Result)
            {
                return BadRequest(response.Message);
            }

            return CreatedAtAction("PostEvent", response.EventId);
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
            _logger.Information($"Editing Event with Id: {id}.");
            var response = _eventsService.UpdateEvent(id, @event);

            if (!response.Result)
            {
                return BadRequest(response.Message);
            }
            
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
            _logger.Information($"Deleting Event with id: {id}");
            var response = _eventsService.DeleteEvent(id);

            if (!response.Result)
            {
                _logger.Error(LoggingMessages.EventNotFound(id));
                return NotFound(response.Errors);
            }
            
            _logger.Information($"The Event with Id: {id} has been successfully deleted.");
            return Ok();
        }
    }
}

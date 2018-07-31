using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
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

            _logger.Information(searchRequest.ToString());
            var response = _eventsService.GetEvents(searchRequest);

            if (!response.Result)
                return BadRequest(response.Errors);

            if (response.Events.Count == 0)
                return NotFound(searchRequest);

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
                if (response.Errors.Count == 0)
                    return NotFound($"The Event with Id: {idRequest.Id} was not found.");

                return BadRequest(response.Errors);
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
            var postRequest = new PostRequest<Event>() { Payload = @event };
            var response = _eventsService.CreateEvent(postRequest);

            if (!response.Result)
                return BadRequest(response.Errors);

            return CreatedAtAction("PostEvent", response.Event.Id);
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
            var putRequest = new PutRequest<Event>() { Id = id, Payload = @event };
            _logger.Information($"Editing Event with Id: {putRequest.Id}.");
            var response = _eventsService.UpdateEvent(putRequest);

            if (!response.Result)
                return BadRequest(response.Errors);

            return Ok(response.Event);
        }

        /// <summary>
        /// Deletes an individual Event.
        /// </summary>
        /// <param name="id">The Event id.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var idRequest = new IdRequest() { Id = id };
            _logger.Information($"Deleting Event with id: {idRequest.Id}");
            var response = _eventsService.DeleteEvent(idRequest);

            if (!response.Result)
            {
                if (response.Errors.Count == 0)
                    return NotFound($"The Event with Id: {idRequest.Id} was not found.");

                return BadRequest(response.Errors);
            }

            return Ok();
        }
    }
}

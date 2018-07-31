using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Messages;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EventSourceWebApi.Controllers
{
    [Route("api/[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly IPlacesService _placeServices;
        private readonly ILogger _logger;

        public PlacesController(IPlacesService placeServices, ILogger logger)
        {
            _placeServices = placeServices;
            _logger = logger;
        }

        /// <summary>
        /// Gets all places 
        /// </summary>
        /// <returns>Collection of Places</returns>
        ///
        [HttpGet]
        public IActionResult GetAllPlaces(string name, string location, string city)
        {
            var request = new PlaceSearchRequest() { Name = name, Location = location, City = city };
            var response = _placeServices.GetAllPlaces(request);
            if (!response.Result)
            {
                return BadRequest(response.Errors);
            }
            _logger.Information("The Places has been successfully taken.");
            return Ok(response.Places);
        }

        /// <summary>
        /// Gets place by id 
        /// </summary>
        /// <param name="id">Place.id</param>
        /// <returns>Returns an individual Place</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var request = new IdRequest { Id = id };
            _logger.Information(LoggingMessages.GettingPlaceById(id));
            var response = _placeServices.GetPlace(request);
            if (!response.Result)
            {
                return BadRequest();
            }
            _logger.Error($"The place with id:{id} has been successfully taken.");
            return Ok(response.Place);
        }

        /// <summary>
        /// Creates a new Place
        /// </summary>
        /// <param name="value">The Place object</param>
        [HttpPost]
        public IActionResult Post([FromBody]Place place)
        {
            var request = new PostRequest<Place> { Payload = place };
            var response = _placeServices.CreatePlace(request);

            if (!response.Result)
            {
                return BadRequest(response.Errors);
            }

            _logger.Information(LoggingMessages.PlaceSucessfullyCreated(response.Place.Id));
            return CreatedAtAction("Post", response.Place.Id);
        }

        /// <summary>
        /// Updates an individual Place
        /// </summary>
        /// <param name="id">Place.Id</param>
        /// <param name="value">Place object</param>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Place place)
        {
            _logger.Information($"Editing place with id: {id}");
            var request = new PutRequest<Place> { Payload = place };
            var placeResponse = _placeServices.UpdatePlace(request);

            if (!placeResponse.Result)
            {
                return BadRequest(placeResponse.Errors);
            }

            _logger.Information($"Place with id: {placeResponse.Place.Id} is succesffuly edited");
            return Ok(placeResponse.Place.Id);
        }

        /// <summary>
        /// Deletes an individual Place
        /// </summary>
        /// <param name="id">Place.Id</param>
        /// 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.Information($"Deleting place with id: {id}");
            var request = new IdRequest() { Id = id };
            var response = _placeServices.DeletePlace(request); 
            if (!response.Result)
            {
                return BadRequest();
            }
            _logger.Information($"The Place with id: {id} has been successfully delited");
            return Ok();
        }
    }
}
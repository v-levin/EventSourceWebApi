using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Messages;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        ///
        /// </summary>
        /// <returns></returns>
        ///
        [HttpGet]
        public IActionResult GetAllPlaces(string keyword)
        {
            var placeRequest = new Request() { Keyword = keyword };
;           var allPlaces = _placeServices.GetAllPlaces(placeRequest);
            return Ok(allPlaces);
        }

        /// <summary>
        /// Getting place by id 
        /// </summary>
        /// <param name="id">Place.id</param>
        /// <returns>Returns an individual Place</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _logger.Information(LoggingMessages.GettingPlaceById(id));
            var getPlaceResponse = _placeServices.GetPlace(id);
            if (getPlaceResponse.Place == null)
            {
                _logger.Error($"The place with id:{id} doesn't exist");
                return BadRequest();
            }
            return Ok(getPlaceResponse.Place);
        }

        /// <summary>
        /// Creates a new Place
        /// </summary>
        /// <param name="value">The Place object</param>
        [HttpPost]
        public IActionResult Post([FromBody] Place place)
        {
            _logger.Information(LoggingMessages.CreatingPlace);
            var result = _placeServices.CreatePlace(place);

                if (!result.Result)
                {
                    
                    return BadRequest(result);
                }
            

            //if (!ModelState.IsValid)
            //{
            //    _logger.Error(LoggingMessages.InvalidInputs(ModelState.ErrorCount));
            //    return BadRequest(ModelState);
            //}
            _logger.Information(LoggingMessages.PlaceSucessfullyCreated(place.Id));
            return CreatedAtAction("Post", place);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Place place)
        {
            _logger.Information($"Editing place with id: {id}");
            if (!ModelState.IsValid)
            {
                _logger.Error(ModelState.ErrorCount + " Invalid input/s");
                return BadRequest(ModelState);
            }
            _placeServices.UpdatePlace(place, id);
            _logger.Information(place.Name + " is succesffuly edited");
            return Ok(place);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Place.Id</param>
        /// 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.Information($"Deleting place with id: {id}");
            var isDeleted = _placeServices.DeletePlace(id);
            //if (!isDeleted)
            //{
            //    _logger.Error($"Place with id: {id} doesn't exist");
            //    return BadRequest();
            //}
            _logger.Information($"Place with id: {id} is succesffuly deleted");
            return Ok();
        }
    }
}
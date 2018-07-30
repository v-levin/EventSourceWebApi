﻿using System;
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
        /// Gets all places 
        /// </summary>
        /// <returns>Collection of Places</returns>
        ///
        [HttpGet]
        public IActionResult GetAllPlaces(PlaceRequest placeRequest)
        {
            //ToDo: validate limit Size
            var placeResponse = _placeServices.GetAllPlaces(placeRequest);
            if (!placeResponse.Result)
            {
                return BadRequest(placeResponse.Errors);
            }
            return Ok(placeResponse.Places);
        }

        /// <summary>
        /// Gets place by id 
        /// </summary>
        /// <param name="id">Place.id</param>
        /// <returns>Returns an individual Place</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _logger.Information(LoggingMessages.GettingPlaceById(id));
            var placeResponse = _placeServices.GetPlace(id);
            if (!placeResponse.Result)
            {
                _logger.Error($"The place with id:{id} doesn't exist");
                return BadRequest();
            }
            return Ok(placeResponse.Place);
        }

        /// <summary>
        /// Creates a new Place
        /// </summary>
        /// <param name="value">The Place object</param>
        [HttpPost]
        public IActionResult Post([FromBody] Place place)
        {
            _logger.Information(LoggingMessages.CreatingPlace);
            var placeResponse = _placeServices.CreatePlace(place);

            if (!placeResponse.Result)
            {
                return BadRequest(placeResponse.Errors);
            }

            _logger.Information(LoggingMessages.PlaceSucessfullyCreated(placeResponse.PlaceId));
            return CreatedAtAction("Post", placeResponse.PlaceId);
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

            var placeResponse = _placeServices.UpdatePlace(place, id);

            if (!placeResponse.Result)
            {
                return BadRequest(placeResponse.Errors);
            }
           
            _logger.Information($"Place with id: {placeResponse.PlaceId} is succesffuly edited");
            return Ok(placeResponse.PlaceId);
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
            var deletePlaceResponse  = _placeServices.DeletePlace(id);
            if(!deletePlaceResponse.Result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
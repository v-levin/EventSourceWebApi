using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSourceWebApi.Controllers
{
    [Route("api/[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly IPlacesService _placeServices;

        public PlacesController(IPlacesService placeServices)
        {
            _placeServices = placeServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ///
        [HttpGet]
        public IEnumerable<Place> GetAll()
        {
            var _allPlaces = _placeServices.GetAll();
            return _allPlaces;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var _place = _placeServices.Get(id);
            if (_place == null)
            {
                return BadRequest(_place);
            }
            return Ok(_place);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public IActionResult Post([FromBody] Place place)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _placeServices.Save(place);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _placeServices.Edit(place, id);
            return Ok(place);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _placeServices.Delete(id);
            if (!isDeleted)
            {
                return BadRequest($"place with id: {id} doesn't exist");
            }
            return Ok();
        }
    }
}
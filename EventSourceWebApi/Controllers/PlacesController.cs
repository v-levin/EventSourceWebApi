using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourceWebApi.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSourceWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Places")]
    public class PlacesController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ///
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "public room", "mkc" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            return Ok();
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

            return CreatedAtAction("Get", new { id = place.Id });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut]
        public void Put(int id, [FromBody] Place place)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// 
        [HttpDelete]
        public void Delete(int id)
        {

        }

    }
}
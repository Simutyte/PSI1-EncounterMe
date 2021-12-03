using MapPinAPI.Models;
using MapPinAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapPinsController : ControllerBase
    {
        private readonly IMapPinRepository _mapPinRepository;

        public MapPinsController(IMapPinRepository mapPinRepository)
        {
            _mapPinRepository = mapPinRepository;
        }

        //Visų objektų gavimas
        [HttpGet]
        public async Task<IEnumerable<MapPin>> GetMapPins()
        {
            return await _mapPinRepository.Get();
        }

        //vieno objekto gavimas pagal id
        [HttpGet("{id}")]
        public async Task<ActionResult<MapPin>> GetMapPins(int id)
        {
            return await _mapPinRepository.Get(id);
        }

        //vieno objekto įrašymas į db
        [HttpPost]
        public async Task<ActionResult<MapPin>> PostMapPins([FromBody] MapPin mapPin)
        {
            var newMapPin = await _mapPinRepository.Create(mapPin);
            return CreatedAtAction(nameof(GetMapPins), new { id = newMapPin.Id }, newMapPin);
        }

        //Užkomentuotas nes čia yra update vieno mapPin. Pas mus nelabai būtų naudojamas
        /*[HttpPut]
        public async Task<ActionResult> PutMapPins(int id, [FromBody] MapPin mapPin)
        {
            if (id != mapPin.Id)
            {
                return BadRequest();
            }

            await _mapPinRepository.Update(mapPin);

            return NoContent();
        }*/

        //Vieno objekto ištrynimas iš db
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var mapPinToDelete = await _mapPinRepository.Get(id);
            if (mapPinToDelete == null)
                return NotFound();

            await _mapPinRepository.Delete(mapPinToDelete.Id);
            return NoContent();
            
        }
    }
}

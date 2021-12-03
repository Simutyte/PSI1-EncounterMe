using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapPinAPI.Models;
using MapPinAPI.Repositories;

namespace MapPinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteMapPinsController : ControllerBase
    {
        private readonly IFavouriteMapPinRepository _repository;

        public FavouriteMapPinsController(IFavouriteMapPinRepository fmRepository)
        {
            _repository = fmRepository;
        }

        // GET: api/FavouriteMapPins
        // Visų FavouriteMapPins gavimas
        [HttpGet]
        public async Task<IEnumerable<FavouriteMapPin>> GetUserMapPins()
        {
            return await _repository.Get();
        }

        // GET: api/FavouriteMapPins/5
        // Sąrašo FavouriteMapPins pagal id gavimas (gaunam visus userio kurio id = id, pamėgtų objektų id)
        [HttpGet("{id}")]
        public async Task<IEnumerable<FavouriteMapPin>> GetUserMapPin(int id)
        {
            return await _repository.Get(id);
        }



        // POST: api/FavouriteMapPins
        // FavouriteMapPins įrašymas į db
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FavouriteMapPin>> PostUserMapPin([FromBody] FavouriteMapPin user)
        {
            
            var newUserMapPin = await _repository.Create(user);
            if (newUserMapPin != null)
                return CreatedAtAction(nameof(GetUserMapPins),  newUserMapPin);
            else
                return new StatusCodeResult(StatusCodes.Status404NotFound);

        }

        // DELETE: api/FavouriteMapPins/5,2
        // FavouriteMapPin ištrynimas
        [HttpDelete("{id1},{id2}")]
        public async Task<IActionResult> DeleteUserMapPin(int id1, int id2)
        {

            await _repository.Delete(id1, id2);
            return NoContent();
        }

    }
}

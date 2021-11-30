﻿using System;
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
    public class UserMapPinsController : ControllerBase
    {
        private readonly IUMRepository _repository;

        public UserMapPinsController(IUMRepository umRepository)
        {
            _repository = umRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<UserMapPin>> GetUserMapPins()
        {
            return await _repository.Get();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<UserMapPin>> GetUserMapPin(int id)
        {
            return await _repository.Get(id);
        }



        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserMapPin>> PostUserMapPin([FromBody] UserMapPin user)
        {
            
            var newUserMapPin = await _repository.Create(user);
            if (newUserMapPin != null)
                return CreatedAtAction(nameof(GetUserMapPins),  newUserMapPin);
            else
                return new StatusCodeResult(StatusCodes.Status404NotFound);

        }

        // DELETE: api/Users/5
        [HttpDelete("{id1},{id2}")]
        public async Task<IActionResult> DeleteUserMapPin(int id1, int id2)
        {

            await _repository.Delete(id1, id2);
            return NoContent();
        }

    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapPinAPI.Models;
using MapPinAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MapPinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationsController : ControllerBase
    {
        private readonly IEvaluationRepository _repository;

        public EvaluationsController(IEvaluationRepository eRepository)
        {
            _repository = eRepository;
        }

        // GET: api/Evaluations/5,2
        // 1 Evaluation gavimas
        [HttpGet("{id1},{id2}")]
        public async Task<ActionResult<Evaluation>> GetEvaluation(int id1, int id2)
        {
            var resp = await _repository.GetOne(id1, id2);
            return resp;
        }

        // GET: api/Evaluations/5
        // Sąrašo Evaluations Pagal MapPinId gavimas - gaunam visus MapPin įvetinimus pagal jo id
        [HttpGet("{id}")]
        public async Task<IEnumerable<Evaluation>> GetEvaluations(int id)
        {
            return await _repository.Get(id);
        }

        // PUT: api/Evaluations/5,4
        // Vieno įvertinimo update
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id1},{id2}")]
        public async Task<IActionResult> PutEvaluation(int id1, int id2, [FromBody] Evaluation evaluation)
        {
            if (id1 != evaluation.UserId || id2 != evaluation.MapPinId )
            {
                return BadRequest();
            }

            await _repository.Update(evaluation);

            return NoContent();
        }



        // POST: api/Evaluations
        // Evaluations įrašymas į db
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evaluation>> PostEvaluation([FromBody] Evaluation evaluation)
        {

            var newevaluation = await _repository.Create(evaluation);
            if (newevaluation != null)
                return CreatedAtAction(nameof(PostEvaluation),evaluation);
            else
                return new StatusCodeResult(StatusCodes.Status404NotFound);

        }

        // DELETE: api/Evaluations/5,2
        // Evaluation ištrynimas
        [HttpDelete("{id1},{id2}")]
        public async Task<IActionResult> DeleteEvaluation(int id1, int id2)
        {

            await _repository.Delete(id1, id2);
            return NoContent();
        }
    }
}

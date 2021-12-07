// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapPinAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MapPinAPI.Repositories
{
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly MapPinContext _context;

        public EvaluationRepository(MapPinContext context)
        {
            _context = context;
        }

        //Evaluation pridėjimas
        public async Task<Evaluation> Create(Evaluation evaluation)
        {
            _context.Evaluations.Add(evaluation);
            await _context.SaveChangesAsync();

            return evaluation;
        }

        //Evaluation ištrynimas
        public async Task Delete(int UserId, int MapPinId)
        {
            var evaluationToDelete = await _context.Evaluations.FindAsync(UserId, MapPinId);
            _context.Evaluations.Remove(evaluationToDelete);
            await _context.SaveChangesAsync();
        }

        //1 Evaluation gavimas
        public async Task<Evaluation> GetOne(int UserId, int MapPinId)
        {
            var evaluationToFind = await _context.Evaluations.FindAsync(UserId, MapPinId);
            if (evaluationToFind != null)
                return evaluationToFind;
            else return null;
            //return await _context.Evaluations.ToListAsync();
        }

        //Gaunam sąrąšą Evaluations, pagal mapPinId - kitaip sakant visus objekto įvertinimus
        public async Task<IEnumerable<Evaluation>> Get(int MapPinId)
        {
            var userMapPins = _context.Evaluations
                .FromSqlInterpolated($"SELECT * FROM Evaluations WHERE MapPinId={MapPinId}").ToListAsync();

            return await userMapPins;
        }

        public async Task Update(Evaluation evaluation)
        {
            _context.Entry(evaluation).State = EntityState.Modified;
            await _context.SaveChangesAsync(); ;
        }
    }
}

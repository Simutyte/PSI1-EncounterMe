// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapPinAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MapPinAPI.Repositories
{
    public class FavouriteMapPinRepository : IFavouriteMapPinRepository
    {
        private readonly MapPinContext _context;

        public FavouriteMapPinRepository(MapPinContext context)
        {
            _context = context;
        }

        //FavouriteMapPin pridėjimas
        public async Task<FavouriteMapPin> Create(FavouriteMapPin userMapPin)
        {
            _context.FavouriteMapPins.Add(userMapPin);
            await _context.SaveChangesAsync();

            return userMapPin;
        }

        //FavouriteMapPin ištrynimas
        public async Task Delete(int UserId, int MapPinId)
        {
            var userMapPinToDelete = await _context.FavouriteMapPins.FindAsync(UserId, MapPinId);
            _context.FavouriteMapPins.Remove(userMapPinToDelete);
            await _context.SaveChangesAsync();
        }

        //Visų FavouriteMapPins gavimas
        public async Task<IEnumerable<FavouriteMapPin>> Get()
        {

            return await _context.FavouriteMapPins.ToListAsync();
        }

        //Gaunam sąrąšą FavouriteMapPins, kur userio id yra UserId
        public async Task<IEnumerable<FavouriteMapPin>> Get(int UserId)
        {
            var userMapPins = _context.FavouriteMapPins
                .FromSqlInterpolated($"SELECT * FROM FavouriteMapPins WHERE UserId={UserId}").ToListAsync();

            return await userMapPins;
        }

      

    }
}

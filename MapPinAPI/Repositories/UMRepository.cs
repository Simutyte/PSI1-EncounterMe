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
    public class UMRepository : IUMRepository
    {
        private readonly MapPinContext _context;

        public UMRepository(MapPinContext context)
        {
            _context = context;
        }

        //puslapy tai yra post - sukuria ir prideda į duomenų bazę naują mapPin
        /*public async Task<UserMapPin> Create(UserMapPin userMapPin)
        {
            _context.UserMapPins.Add(userMapPin);
            await _context.SaveChangesAsync();

            return userMapPin;
        }

        public async Task Delete(int UserId, int MapPinId)
        {
            var userMapPinToDelete = await _context.UserMapPins.FindAsync(UserId, MapPinId);
            _context.UserMapPins.Remove(userMapPinToDelete);
            await _context.SaveChangesAsync();
        }

        //gauna sąrašą visų mapPin
        //kadangi mapPin turi address ir hours kurie yra reference tipo tai kiekvienam MapPin mes juos užloadinam. Kitaip prie address ir hours rodytų null
        public async Task<IEnumerable<UserMapPin>> Get()
        {

            return await _context.UserMapPins.ToListAsync();
        }

        //pagal id gaunam vieną mapPin. Vykdom užloadinimą dėl tų pačių priežasčių kaip ir kitam get
        public IEnumerable<UserMapPin> Get(int UserId)
        {

            var userMapPins = _context.UserMapPins
                .FromSqlRaw("SELECT * FROM UserMapPins WHERE UserMapPins.UserId = @id", UserId);

            return userMapPins;
        }*/

    }
}

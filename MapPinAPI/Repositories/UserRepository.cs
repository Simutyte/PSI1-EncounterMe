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
    public class UserRepository : IUserRepository
    {
        private readonly MapPinContext _context;

        public UserRepository(MapPinContext context)
        {
            _context = context;
        }

        //visų gavimas
        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        //vieno gavimas
        public async Task<User> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);

            //_context.Users.FromSqlRaw("SELECT FROM ")
            if (user == null)
            {
                return null;
            }

            return user;
        }

        //user pridėjimas
        public async Task<User> Create(User user)
        {
            if(!RegistrationValidate(user))
            {
                _context.Users.Add(user);
                
                await _context.SaveChangesAsync();

                return user;
            }
            return null;
        }

        //User ištrynimas
        public async Task Delete(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        //Userio update
        public async Task Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(); ;
        }

        //registracijos patikrinimui
        private bool RegistrationValidate(User user)
        {
            return _context.Users.Any(e => e.Username == user.Username || e.Email == user.Email);
        }
    }
}

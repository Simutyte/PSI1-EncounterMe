using MapPinAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI.Repositories
{
    public class MapPinRepository : IMapPinRepository
    {
        private readonly MapPinContext _context;

        public MapPinRepository(MapPinContext context)
        {
            _context = context;
        }

        //puslapy tai yra post - sukuria ir prideda į duomenų bazę naują mapPin
        public async Task<MapPin> Create(MapPin mapPin)
        {
            _context.MapPins.Add(mapPin);
            await _context.SaveChangesAsync();

            return mapPin;
        }

        public async Task Delete(int id)
        {
            var mapPinToDelete = await _context.MapPins.FindAsync(id);
            var addressToDelete = await _context.Addresses.FindAsync(mapPinToDelete.Address.AddressID);

            _context.Addresses.Remove(addressToDelete);
            _context.MapPins.Remove(mapPinToDelete);

            await _context.SaveChangesAsync();
        }

        //gauna sąrašą visų mapPin
        //kadangi mapPin turi address ir hours kurie yra reference tipo tai kiekvienam MapPin mes juos užloadinam. Kitaip prie address ir hours rodytų null
        public async Task<IEnumerable<MapPin>> Get()
        {
            
           foreach( var MapPin in _context.MapPins)
            {
                _context.Entry(MapPin).Reference(a => a.Address).Load();
            }
            return await _context.MapPins.ToListAsync();
        }

        //pagal id gaunam vieną mapPin. Vykdom užloadinimą dėl tų pačių priežasčių kaip ir kitam get
        public async Task<MapPin> Get(int id)
        {
            var MapPin = await _context.MapPins.FindAsync(id);
            _context.Entry(MapPin).Reference(a => a.Address).Load();
            return await _context.MapPins.FindAsync(id);
        }

        // mapPin atnaujinimui. užkomentuotas nes pas mus nelabai būtų naudojamas
        /*public async Task Update(MapPin mapPin)
        {
            _context.Entry(mapPin).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }*/
    }
}


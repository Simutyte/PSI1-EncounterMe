using MapPinAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI.Repositories
{
    public interface IMapPinRepository
    {
        Task<IEnumerable<MapPin>> Get();

        Task<MapPin> Get(int id);

        Task<MapPin> Create(MapPin mapPin);

        //užkomentuotas nes čia mapPin atnaujinimas. Pas mus nelabai būtų naudojamas
        //Task Update(MapPin mapPin);

        Task Delete(int id);
    }
}

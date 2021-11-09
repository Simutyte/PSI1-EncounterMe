using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapPinAPI.Models
{
    public class MapPinContext : DbContext
    {
        //čia yra mūsų lentelės duomenų bazėj
        public DbSet<MapPin> MapPins { get; set; }
        public DbSet<Address> Addresses { get; set; }

        //patikrina ar duomenų bazė sukurta
        public MapPinContext(DbContextOptions<MapPinContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        //nurodom ryšius tarp lentelių
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MapPin>()
                .HasOne<Address>(s => s.Address);
        
        }

    }
}

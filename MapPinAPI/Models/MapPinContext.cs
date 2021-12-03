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
        public DbSet<User> Users { get; set; }
        public DbSet<FavouriteMapPin> FavouriteMapPins{ get; set; }

        //patikrina ar duomenų bazė sukurta
        public MapPinContext(DbContextOptions<MapPinContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        //nurodom ryšius tarp lentelių
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nustatom jog primary key bus 2 stulpeliai
            modelBuilder.Entity<FavouriteMapPin>()
                .HasKey(f => new { f.UserId, f.MapPinId });

            //MapPin ir address ryšys
            /*modelBuilder.Entity<MapPin>()
                .HasOne<Address>(s => s.Address);

            modelBuilder.Entity<MapPin>()
                .HasOne<User>(u => u.Creator);

            modelBuilder.Entity<User>()
                .HasMany(u => u.OwnedObjects)
                .WithOne(m => m.Creator);*/

            //User ir MapPin ryšys - owner
            /*modelBuilder.Entity<MapPin>()
                .HasOne<User>(s => s.Owner)
                .WithMany(g => g.OwnedObjects)
                .HasForeignKey(s => s.OwnerId);

            modelBuilder.Entity<User>()
                .HasMany<MapPin>(g => g.OwnedObjects)
                .WithOne(s => s.Owner)
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavouriteMapPin>()
                .HasKey(f => new { f.UserId, f.MapPinId });

            modelBuilder.Entity<User>()
                .HasMany<FavouriteMapPin>(f => f.Favourites);

            modelBuilder.Entity<MapPin>()
                .HasMany<FavouriteMapPin>(f => f.FavouritesMapPins);
            modelBuilder.Entity<FavouriteMapPin>()
                .HasOne<User>(f => f.User)
                .WithMany(u => u.Favourites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<FavouriteMapPin>()
                .HasOne<MapPin>(f => f.MapPin)
                .WithMany(u => u.FavouritesMapPins)
                .HasForeignKey(f => f.MapPinId);*/
        }

    }
}

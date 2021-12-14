// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapPinAPI.Controllers;
using MapPinAPI.Models;
using MapPinAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Test1.Tests
{
    public class MapPinDbTests
    {
        [Fact]
        public async Task MapPinDbSet_UsingInMemoryProvider_SimpleActionsTesting()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "MapPinTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var MapPin1 = new MapPin() { Name = "Pirmas", Description = "Pirmas", ImageName = "pirmasPhoto" };
                var MapPin2 = new MapPin() { Name = "Antras", Description = "Antras", ImageName = "antrasPhoto" };
                context.MapPins.Add(MapPin1);
                context.MapPins.Add(MapPin2);
                await context.SaveChangesAsync();
            }

            using (var context = new MapPinContext(options))
            {
                //tikrinam pridėjimą
                var count = await context.MapPins.CountAsync();
                Assert.Equal(2, count);

                //tikrinam paiešką
                var u = await context.MapPins.FirstOrDefaultAsync(MapPin => MapPin.Name == "Pirmas");
                Assert.NotNull(u);

                //tikrinam update
                u.Description = "Trecias";
                context.MapPins.Update(u);
                await context.SaveChangesAsync();
                var u1 = await context.MapPins.FirstOrDefaultAsync(MapPin => MapPin.Description == "Trecias");
                Assert.NotNull(u1);

                //tikrinam ištrynimą
                context.MapPins.Remove(u);
                await context.SaveChangesAsync();
                var u2 = await context.MapPins.FirstOrDefaultAsync(MapPin => MapPin.ImageName == "pirmasPhoto");
                Assert.Null(u2);

                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public async Task MapPinsController_GetMapPins_TypeOfGetAllMapPinsIsCorrect()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "MapPinTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var mockRepo = new Mock<IMapPinRepository>();
                var controller = new MapPinsController(mockRepo.Object);
                var result = await controller.GetMapPins();

                Assert.IsAssignableFrom<IEnumerable<MapPin>>(result);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task MapPinsController_GetMapPin_TypeOfGetMapPinIsCorrect()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "MapPinTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var MapPin1 = new MapPin() { Name = "Pirmas", Description = "Pirmas", ImageName = "pirmasPhoto" };
                context.MapPins.Add(MapPin1);
                await context.SaveChangesAsync();

                var repo = new MapPinRepository(context);
                var controller = new MapPinsController(repo);
                var result = await controller.GetMapPins(1);

                Assert.IsType<ActionResult<MapPin>>(result);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task MapPinsController_GetMapPins_GetsRightAmountOfMapPins()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "MapPinTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var MapPin1 = new MapPin() { Name = "Pirmas", Description = "Pirmas", ImageName = "pirmasPhoto" };
                var MapPin2 = new MapPin() { Name = "Antras", Description = "Antras", ImageName = "antrasPhoto" };
                context.MapPins.Add(MapPin1);
                context.MapPins.Add(MapPin2);
                await context.SaveChangesAsync();

                var repo = new MapPinRepository(context);
                var controller = new MapPinsController(repo);
                var result = await controller.GetMapPins();

                Assert.Equal(2, result.Count());
                context.Database.EnsureDeleted();
            }
        }

        public async Task AddressDbSet_UsingInMemoryProvider_SimpleActionsTesting()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "AddressTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var address1 = new Address() { City = "Pirmas", Country = "Pirmas", PostalCode = "pirmasP" };
                var address2 = new Address() { City = "Antras", Country = "Antras", PostalCode = "antrasP" };
                context.Addresses.Add(address1);
                context.Addresses.Add(address2);
                await context.SaveChangesAsync();
            }

            using (var context = new MapPinContext(options))
            {
                //tikrinam pridėjimą
                var count = await context.Addresses.CountAsync();
                Assert.Equal(2, count);

                //tikrinam paiešką
                var u = await context.Addresses.FirstOrDefaultAsync(MapPin => MapPin.City == "Pirmas");
                Assert.NotNull(u);

                //tikrinam update
                u.Country = "Trecias";
                context.Addresses.Update(u);
                await context.SaveChangesAsync();
                var u1 = await context.Addresses.FirstOrDefaultAsync(MapPin => MapPin.Country == "Trecias");
                Assert.NotNull(u1);

                //tikrinam ištrynimą
                context.Addresses.Remove(u);
                await context.SaveChangesAsync();
                var u2 = await context.Addresses.FirstOrDefaultAsync(MapPin => MapPin.PostalCode == "pirmasP");
                Assert.Null(u2);

                context.Database.EnsureDeleted();
            }

        }
    }
}

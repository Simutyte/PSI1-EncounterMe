// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapPinAPI.Controllers;
using MapPinAPI.Repositories;
using MapPinAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Test1.Tests
{
    public class FavouriteMapPinDbTests
    {
        [Fact]
        public async Task FavouriteMapPinDbSet_UsingInMemoryProvider_SimpleActionsTesting()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "FavouriteMapPinTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var FavouriteMapPin1 = new FavouriteMapPin() { MapPinId = 1, UserId = 1};
                var FavouriteMapPin2 = new FavouriteMapPin() { MapPinId = 2, UserId = 1 };
                context.FavouriteMapPins.Add(FavouriteMapPin1);
                context.FavouriteMapPins.Add(FavouriteMapPin2);
                await context.SaveChangesAsync();
            }

            using (var context = new MapPinContext(options))
            {
                //tikrinam pridėjimą
                var count = await context.FavouriteMapPins.CountAsync();
                Assert.Equal(2, count);

                //tikrinam paiešką
                var u = await context.FavouriteMapPins.FirstOrDefaultAsync(FavouriteMapPin => FavouriteMapPin.MapPinId == 1);
                Assert.NotNull(u);

                //tikrinam ištrynimą
                context.FavouriteMapPins.Remove(u);
                await context.SaveChangesAsync();
                var u2 = await context.FavouriteMapPins.FirstOrDefaultAsync(FavouriteMapPin => FavouriteMapPin.MapPinId == 1);
                Assert.Null(u2);

                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public async Task FavouriteMapPinsController_GetFavouriteMapPins_TypeOfGetAllFavouriteMapPinsIsCorrect()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "FavouriteMapPinTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var mockRepo = new Mock<IFavouriteMapPinRepository>();
                var controller = new FavouriteMapPinsController(mockRepo.Object);
                var result = await controller.GetUserMapPins();

                Assert.IsAssignableFrom<IEnumerable<FavouriteMapPin>>(result);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task FavouriteMapPinsController_GetFavouriteMapPin_TypeOfGetFavouriteMapPinIsCorrect()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "FavouriteMapPinTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var FavouriteMapPin1 = new FavouriteMapPin() { MapPinId = 1, UserId = 1 };
                context.FavouriteMapPins.Add(FavouriteMapPin1);
                await context.SaveChangesAsync();

                var repo = new FavouriteMapPinRepository(context);
                var controller = new FavouriteMapPinsController(repo);
                var result = await controller.GetUserMapPin(1);

                Assert.IsAssignableFrom<IEnumerable<FavouriteMapPin>>(result);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task FavouriteMapPinsController_GetFavouriteMapPins_GetsRightAmountOfFavouriteMapPins()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "FavouriteMapPinTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var FavouriteMapPin1 = new FavouriteMapPin() { MapPinId = 1, UserId = 1 };
                var FavouriteMapPin2 = new FavouriteMapPin() { MapPinId = 2, UserId = 1 };
                context.FavouriteMapPins.Add(FavouriteMapPin1);
                context.FavouriteMapPins.Add(FavouriteMapPin2);
                await context.SaveChangesAsync();

                var repo = new FavouriteMapPinRepository(context);
                var controller = new FavouriteMapPinsController(repo);
                var result = await controller.GetUserMapPin(1);

                Assert.Equal(2, result.Count());
                context.Database.EnsureDeleted();
            }
        }
    }
}

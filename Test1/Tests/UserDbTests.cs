// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapPinAPI.Controllers;
using MapPinAPI.Models;
using MapPinAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Xunit.Priority;

namespace Test1.Tests
{
    public class UserDbTests
    {
        [Fact]
        public async Task UserDbSet_UsingInMemoryProvider_SimpleActionsTesting()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var user1 = new User() { Username = "Pirmas", Password = "Pirmas", Email = "pirmas@sample.com" };
                var user2 = new User() { Username = "Antras", Password = "Antras", Email = "antras@sample.com" };
                context.Users.Add(user1);
                context.Users.Add(user2);
                await context.SaveChangesAsync();
            }

            using (var context = new MapPinContext(options))
            {
                //tikrinam pridėjimą
                var count = await context.Users.CountAsync();
                Assert.Equal(2, count);

                //tikrinam paiešką
                var u = await context.Users.FirstOrDefaultAsync(user => user.Email == "pirmas@sample.com");
                Assert.NotNull(u);

                //tikrinam update
                u.Username = "Trecias";
                context.Users.Update(u);
                await context.SaveChangesAsync();
                var u1 = await context.Users.FirstOrDefaultAsync(user => user.Username == "Trecias");
                Assert.NotNull(u1);

                //tikrinam ištrynimą
                context.Users.Remove(u);
                await context.SaveChangesAsync();
                var u2 = await context.Users.FirstOrDefaultAsync(user => user.Email == "pirmas@sample.com");
                Assert.Null(u2);

                context.Database.EnsureDeleted();
            }

        }

        [Fact]
        public async Task UsersController_GetUsers_TypeOfGetAllUsersIsCorrect()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var mockRepo = new Mock<IUserRepository>();
                var controller = new UsersController(mockRepo.Object);
                var result = await controller.GetUsers();

                Assert.IsAssignableFrom<IEnumerable<User>>(result);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task UsersController_GetUser_TypeOfGetUserIsCorrect()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var user1 = new User() { Username = "Pirmas", Password = "Pirmas", Email = "pirmas@sample.com" };
                context.Users.Add(user1);
                await context.SaveChangesAsync();

                var repo = new UserRepository(context);
                var controller = new UsersController(repo);
                var result = await controller.GetUser(1);

                Assert.IsType<ActionResult<User>>(result);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task UsersController_GetUsers_GetsRightAmountOfUsers()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var user1 = new User() { Username = "Pirmas", Password = "Pirmas", Email = "pirmas@sample.com" };
                var user2 = new User() { Username = "Antras", Password = "Antras", Email = "antras@sample.com" };
                context.Users.Add(user1);
                context.Users.Add(user2);
                await context.SaveChangesAsync();

                var repo = new UserRepository(context);
                var controller = new UsersController(repo);
                var result = await controller.GetUsers();

                Assert.Equal(2, result.Count());
                context.Database.EnsureDeleted();
            }
        }

    }
}

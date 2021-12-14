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
    public class EvaluationDbTests
    {
        [Fact]
        public async Task EvaluationDbSet_UsingInMemoryProvider_SimpleActionsTesting()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "EvaluationTestDb1")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var Evaluation1 = new Evaluation() {UserId = 1, MapPinId = 2, Value = 5 };
                var Evaluation2 = new Evaluation() {UserId = 2, MapPinId = 2, Value = 9 };
                context.Evaluations.Add(Evaluation1);
                context.Evaluations.Add(Evaluation2);
                await context.SaveChangesAsync();
            }

            using (var context = new MapPinContext(options))
            {
                //tikrinam pridėjimą
                var count = await context.Evaluations.CountAsync();
                Assert.Equal(2, count);

                //tikrinam paiešką
                var u = await context.Evaluations.FirstOrDefaultAsync(Evaluation => Evaluation.Value == 5);
                Assert.NotNull(u);

                //tikrinam update
                u.Value = 6;
                context.Evaluations.Update(u);
                await context.SaveChangesAsync();
                var u1 = await context.Evaluations.FirstOrDefaultAsync(Evaluation => Evaluation.Value == 6);
                Assert.NotNull(u1);

                //tikrinam ištrynimą
                context.Evaluations.Remove(u);
                await context.SaveChangesAsync();
                var u2 = await context.Evaluations.FirstOrDefaultAsync(Evaluation => Evaluation.Value == 6);
                Assert.Null(u2);

                context.Database.EnsureDeleted();
                context.Dispose();
            }

        }

        [Fact]
        public async Task EvaluationsController_GetEvaluation_TypeOfGetEvaluationIsCorrect()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "EvaluationTestDb2")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var Evaluation1 = new Evaluation() { UserId = 1, MapPinId = 2, Value = 5 };
                context.Evaluations.Add(Evaluation1);
                await context.SaveChangesAsync();

                var mockRepo = new Mock<EvaluationRepository>(context);
                var controller = new EvaluationsController(mockRepo.Object);
                var result = await controller.GetEvaluation(1,2);

                Assert.IsType<ActionResult<Evaluation>>(result);
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task EvaluationsController_GetEvaluations_GetsRightAmountOfEvaluationsAndRightType()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "EvaluationTestDb3")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var Evaluation1 = new Evaluation() { UserId = 1, MapPinId = 2, Value = 5 };
                var Evaluation2 = new Evaluation() { UserId = 2, MapPinId = 2, Value = 9 };
                context.Evaluations.Add(Evaluation1);
                context.Evaluations.Add(Evaluation2);
                await context.SaveChangesAsync();

                var mockRepo = new Mock<EvaluationRepository>(context);
                var controller = new EvaluationsController(mockRepo.Object);
                var result = await controller.GetEvaluations(2);

                Assert.IsAssignableFrom<IEnumerable<Evaluation>>(result);
                Assert.Equal(2, result.Count());
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task EvaluationsController_PostUser_PostIsWorkingCorrectly()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "EvaluationsTestDb4")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var mockRepo = new Mock<EvaluationRepository>(context);
                var controller = new EvaluationsController(mockRepo.Object);
                var Evaluation = new Evaluation() { UserId = 1, MapPinId = 2, Value = 5 };
                var result = await controller.PostEvaluation(Evaluation);

                Assert.IsType<ActionResult<Evaluation>>(result);

                var u = await context.Evaluations.FirstOrDefaultAsync(user => user.Value == 5);
                Assert.NotNull(u);

                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task EvaluationsController_DeleteUser_DeleteIsWorkingCorrectly()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "EvaluationsTestDb5")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var Evaluation = new Evaluation() { UserId = 1, MapPinId = 2, Value = 5 };
                context.Evaluations.Add(Evaluation);
                await context.SaveChangesAsync();

                var mockRepo = new Mock<EvaluationRepository>(context);
                var controller = new EvaluationsController(mockRepo.Object);

                var result = await controller.DeleteEvaluation(Evaluation.UserId, Evaluation.MapPinId);

                Assert.IsAssignableFrom<IActionResult>(result);

                var u = await context.Evaluations.FindAsync(Evaluation.UserId, Evaluation.MapPinId);
                Assert.Null(u);

                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task EvaluationsController_PutEvaluation_PutIsWorkingCorrectly()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "EvaluationsTestDb6")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var Evaluation = new Evaluation() { UserId = 1, MapPinId = 2, Value = 5 };
                context.Evaluations.Add(Evaluation);
                await context.SaveChangesAsync();

                var mockRepo = new Mock<EvaluationRepository>(context);
                var controller = new EvaluationsController(mockRepo.Object);

                Evaluation.Value = 7;

                var result = await controller.PutEvaluation(Evaluation.UserId, Evaluation.MapPinId, Evaluation);

                Assert.IsAssignableFrom<IActionResult>(result);

                var u = await context.Evaluations.FirstOrDefaultAsync(Evaluation => Evaluation.Value == 7);
                Assert.NotNull(u);

                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }
}

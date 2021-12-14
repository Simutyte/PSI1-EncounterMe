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
                .UseInMemoryDatabase(databaseName: "EvaluationTestDb")
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
            }

        }

        [Fact]
        public async Task EvaluationsController_GetEvaluation_TypeOfGetEvaluationIsCorrect()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "EvaluationTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var Evaluation1 = new Evaluation() { UserId = 1, MapPinId = 2, Value = 5 };
                context.Evaluations.Add(Evaluation1);
                await context.SaveChangesAsync();

                var repo = new EvaluationRepository(context);
                var controller = new EvaluationsController(repo);
                var result = await controller.GetEvaluation(1,2);

                Assert.IsType<ActionResult<Evaluation>>(result);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task EvaluationsController_GetEvaluations_GetsRightAmountOfEvaluationsAndRightType()
        {
            var options = new DbContextOptionsBuilder<MapPinContext>()
                .UseInMemoryDatabase(databaseName: "EvaluationTestDb")
                .Options;

            using (var context = new MapPinContext(options))
            {
                var Evaluation1 = new Evaluation() { UserId = 1, MapPinId = 2, Value = 5 };
                var Evaluation2 = new Evaluation() { UserId = 2, MapPinId = 2, Value = 9 };
                context.Evaluations.Add(Evaluation1);
                context.Evaluations.Add(Evaluation2);
                await context.SaveChangesAsync();

                var repo = new EvaluationRepository(context);
                var controller = new EvaluationsController(repo);
                var result = await controller.GetEvaluations(2);

                Assert.IsAssignableFrom<IEnumerable<Evaluation>>(result);
                Assert.Equal(2, result.Count());
                context.Database.EnsureDeleted();
            }
        }
    }
}

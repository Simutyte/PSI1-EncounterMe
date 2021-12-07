// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapPinAPI.Models;

namespace MapPinAPI.Repositories
{
    public interface IEvaluationRepository
    {
        Task<Evaluation> GetOne(int UserId, int MapPinId);

        Task<IEnumerable<Evaluation>> Get(int mapPinId);

        Task<Evaluation> Create(Evaluation evaluation);

        Task Delete(int UserId, int MapPinId);

        Task Update(Evaluation evaluation);
    }
}

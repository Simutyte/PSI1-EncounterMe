// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapPinAPI.Models;

namespace MapPinAPI.Repositories
{
    public interface IUMRepository 
    {
        Task<IEnumerable<UserMapPin>> Get();

        Task<IEnumerable<UserMapPin>> Get(int UserId);

        Task<UserMapPin> Create(UserMapPin mapPin);

        Task Delete(int UserId, int MapPinId);
    }
}

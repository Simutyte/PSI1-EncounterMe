// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapPinAPI.Models;

namespace MapPinAPI.Repositories
{
    public interface IFavouriteMapPinRepository 
    {
        Task<IEnumerable<FavouriteMapPin>> Get();

        Task<IEnumerable<FavouriteMapPin>> Get(int UserId);

        Task<FavouriteMapPin> Create(FavouriteMapPin mapPin);

        Task Delete(int UserId, int MapPinId);
    }
}

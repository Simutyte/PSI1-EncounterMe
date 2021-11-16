// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace EncounterMe.Services
{
    public interface IGpsSettings
    {
        void OpenSettings();
        bool IsGpsEnabled();
    }
}

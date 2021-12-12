// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Serilog;

namespace MapPinAPI.Middleware
{
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();

                Log.Logger.Information($"Method {invocation.Method.Name} " +
                    $"called with these parameters: {JsonConvert.SerializeObject(invocation.Arguments)}" +
                    $"returned this response: {JsonConvert.SerializeObject(invocation.ReturnValue)}");
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Error happened in method: {invocation.Method}. Error: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}

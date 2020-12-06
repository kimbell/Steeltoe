﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Availability;
using System;

namespace Steeltoe.Management.Endpoint
{
    public class AllActuatorsStartupFilter : IStartupFilter
    {
        public AllActuatorsStartupFilter(Action<IEndpointConventionBuilder> configureConventions = null)
        {
            _configureConventions = configureConventions;
        }

        private readonly Action<IEndpointConventionBuilder> _configureConventions;

        public static void InitializeAvailability(IServiceProvider serviceProvider)
        {
            var lifetime = serviceProvider.GetService<IHostApplicationLifetime>();
            var availability = serviceProvider.GetService<ApplicationAvailability>();
            lifetime.ApplicationStarted.Register(() =>
            {
                availability.SetAvailabilityState(availability.LivenessKey, LivenessState.Correct, "ApplicationStarted");
                availability.SetAvailabilityState(availability.ReadinessKey, ReadinessState.AcceptingTraffic, "ApplicationStarted");
            });
            lifetime.ApplicationStopping.Register(() => availability.SetAvailabilityState(availability.ReadinessKey, ReadinessState.RefusingTraffic, "ApplicationStopping"));
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                next(app);

                app.UseEndpoints(endpoints =>
                {
                    var builder = endpoints.MapAllActuators();
                    _configureConventions?.Invoke(builder);
                });

                InitializeAvailability(app.ApplicationServices);
            };
        }
    }
}

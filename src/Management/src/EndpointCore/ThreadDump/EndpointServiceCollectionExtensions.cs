// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Steeltoe.Management.Endpoint.Hypermedia;
using System;

namespace Steeltoe.Management.Endpoint.ThreadDump
{
    public static class EndpointServiceCollectionExtensions
    {
        /// <summary>
        /// Adds components of the Thread Dump actuator to Microsoft-DI
        /// </summary>
        /// <param name="services">Service collection to add actuator to</param>
        /// <param name="config">Application configuration. Retrieved from the <see cref="IServiceCollection"/> if not provided (this actuator looks for a settings starting with management:endpoints:dump)</param>
        public static void AddThreadDumpActuator(this IServiceCollection services, IConfiguration config = null)
        {
            services.AddThreadDumpActuator(config, MediaTypeVersion.V2);
        }

        public static void AddThreadDumpActuator(this IServiceCollection services, IConfiguration config, MediaTypeVersion version)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            config ??= services.BuildServiceProvider().GetService<IConfiguration>();
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.AddActuatorManagementOptions(config);
            if (version == MediaTypeVersion.V1)
            {
                services.AddEndpointEntry<ThreadDumpEndpointOptions, ThreadDumpEndpoint>(config, ThreadDumpEndpointOptions.SECTION_NAME);
            }
            else
            {
                // CRK TODO
                // if (options.Id == "dump")
                // {
                //     options.Id = "threaddump";
                // }
                services.AddEndpointEntry<ThreadDumpEndpointOptions, ThreadDumpEndpoint_v2>(config, ThreadDumpEndpointOptions.SECTION_NAME);
            }

            services.TryAddSingleton<IThreadDumper, ThreadDumper>();
        }
    }
}

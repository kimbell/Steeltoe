using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Steeltoe.Management;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds information about an enpoint
        /// </summary>
        /// <typeparam name="TOptions">The type representing the endpoitn options</typeparam>
        /// <typeparam name="TEndpoint">The endpoint implemenation</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">A reference to the configuration system</param>
        /// <param name="sectionName">The name of the configuration section containing endpoint settings</param>
        /// <returns>Reference to the <see cref="IServiceCollection"/></returns>
        internal static IServiceCollection AddEndpointEntry<TOptions, TEndpoint>(this IServiceCollection services, IConfiguration configuration, string sectionName)
            where TOptions : class, IEndpointOptions
            where TEndpoint : class, IEndpoint
        {
            // register the configuration options for the endpoint
            var managementOptions = configuration.GetSection(ManagementEndpointOptions.SECTION_NAME);
            services.Configure<TOptions>(managementOptions.GetSection(sectionName));

            // register information needed to run endpoint
            services.TryAddScoped<TEndpoint>();
            services.AddSingleton<IEndpointRegistrationEntry>(sp =>
            {
                return new EndpointRegistrationEntry<TOptions, TEndpoint>(sp, (endpoints, endpointConventionBuilder) =>
                {
                    endpoints.Map<TEndpoint>(endpointConventionBuilder);
                });
            });

            return services;
        }
    }
}

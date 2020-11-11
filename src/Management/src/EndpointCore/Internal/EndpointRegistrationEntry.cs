using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Steeltoe.Management.Endpoint.Internal
{
    internal class EndpointRegistrationEntry<TOptions, TEndpoint> : IEndpointRegistrationEntry
        where TOptions : IEndpointOptions
        where TEndpoint: IEndpoint
    {
        private readonly IServiceProvider _serviceProvider;

        public IEndpointOptions Options => _serviceProvider.GetRequiredService<IOptionsMonitor<TOptions>>().CurrentValue;

        public Action<IEndpointRouteBuilder> SetupEndpoint { get; }

        public Type EndpointType => typeof(TEndpoint);

        public EndpointRegistrationEntry(IServiceProvider serviceProvider, Action<IEndpointRouteBuilder> setupEndpoint)
        {
            _serviceProvider = serviceProvider;
            SetupEndpoint = setupEndpoint;
        }
    }

    /// <summary>
    /// Common interface for an endpoint registry entry
    /// </summary>
    internal interface IEndpointRegistrationEntry
    {
        /// <summary>
        /// Gets the options for this endpoint entry
        /// </summary>
        IEndpointOptions Options { get; }

        /// <summary>
        /// Gets the delegate used to add the endoint in ASP.NET
        /// </summary>
        Action<IEndpointRouteBuilder> SetupEndpoint { get; }

        /// <summary>
        /// Gets the type of the endpoint
        /// </summary>
        Type EndpointType { get; }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management;
using Steeltoe.Management.Endpoint.Internal;

namespace System
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Finds the <see cref="IEndpointOptions"/> for the selected middleware
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="typeEndpoint">The type of the endpoint</param>
        /// <returns>A reference to <see cref="IEndpointOptions"/> if found. Otherwise null</returns>
        public static IEndpointOptions FindEndpointOptionsForMiddlewareType(this IServiceProvider serviceProvider, Type typeEndpoint)
        {
            foreach (var entry in serviceProvider.GetServices<IEndpointRegistrationEntry>())
            {
                if (entry.EndpointType == typeEndpoint)
                {
                    return entry.Options;
                }
            }

            return null;
        }
    }
}

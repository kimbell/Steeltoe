// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Steeltoe.Management.Endpoint.Metrics
{
    public class PrometheusEndpointOptions : AbstractEndpointOptions, IPrometheusEndpointOptions, IOptions<PrometheusEndpointOptions>
    {
        /// <summary>
        /// The name of the configuration section when accessed through <see cref="IConfiguration"/>
        /// </summary>
        public const string SECTION_NAME = "prometheus";
        internal const string MANAGEMENT_INFO_PREFIX = "management:endpoints:prometheus";

        public PrometheusEndpointOptions()
            : base()
        {
            Id = "prometheus";
            ExactMatch = false;
        }

        public PrometheusEndpointOptions(IConfiguration config)
            : base(MANAGEMENT_INFO_PREFIX, config)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = "prometheus";
            }

            ExactMatch = false;
        }

        /// <inheritdoc />
        public PrometheusEndpointOptions Value => this; // for testing purposes
    }
}

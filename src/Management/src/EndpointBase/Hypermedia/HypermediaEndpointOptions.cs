// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Steeltoe.Management.Endpoint.Hypermedia
{
    public class HypermediaEndpointOptions : AbstractEndpointOptions, IActuatorHypermediaOptions,
        IOptionsMonitor<HypermediaEndpointOptions>
    {
        /// <summary>
        /// The name of the configuration section when accessed through <see cref="IConfiguration"/>
        /// </summary>
        public const string SECTION_NAME = "actuator";
        private const string MANAGEMENT_INFO_PREFIX = "management:endpoints:actuator";

        public HypermediaEndpointOptions()
            : base()
        {
            Id = string.Empty;
        }

        public HypermediaEndpointOptions(IConfiguration config)
            : base(MANAGEMENT_INFO_PREFIX, config)
        {
        }

        public HypermediaEndpointOptions CurrentValue => this;

        public HypermediaEndpointOptions Get(string name) => this;

        public IDisposable OnChange(Action<HypermediaEndpointOptions, string> listener) => null;
    }
}